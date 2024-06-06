using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Fanex.Data.ProxyClient.Tests")]
namespace Fanex.Data.Vault
{
    using System;
    using System.Threading;

    internal interface VaultPollingService
    {
        void Start(int secondsDelay);

        void Stop();

        Exception LatestException { get; }
    }

    internal class VaultPollingServiceImp : VaultPollingService
    {
        private Thread pollingThread = null;

        private bool running = false;

        private int secondsDelay = 5 * 60;

        private readonly VaultProvider provider;

        internal VaultPollingServiceImp(VaultProvider provider)
        {
            this.provider = provider;
        }

        public Exception LatestException { get; private set; }

        public void Start(int secondsDelay)
        {
            Stop();

            running = true;
            this.secondsDelay = secondsDelay;
            pollingThread = new Thread(Poll);
            pollingThread.Start();
        }

        public void Stop()
        {
            if (pollingThread != null)
            {
                running = false;
                pollingThread.Join();
                pollingThread = null;
            }
        }

        private void Poll()
        {
            var vaultService = new VaultServiceImp(provider);
            while (running)
            {
                SafeReloadDbSettingProvider(vaultService);
                Thread.Sleep(secondsDelay * 1000);
            }
        }

        private void SafeReloadDbSettingProvider(VaultService vaultService)
        {
            try
            {
                var isCredentialChanged = vaultService.UpdateConnectionMaps().GetAwaiter().GetResult();
                if (isCredentialChanged)
                {
                    DbSettingProviderManager.CurrentSession.RestartSettingProvider();
                }
            }
            catch (Exception e)
            {
                LatestException = e;
            }
        }
    }
}
