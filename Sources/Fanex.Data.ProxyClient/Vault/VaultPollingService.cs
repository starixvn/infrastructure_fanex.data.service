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
        private Thread pollingThread;

        private bool running;

        private int delay = 5 * 60;

        private readonly VaultService vaultService;

        internal VaultPollingServiceImp(VaultProvider provider)
        {
            vaultService = new VaultServiceImp(provider);
        }

        public Exception LatestException { get; private set; }

        public void Start(int secondsDelay)
        {
            Stop();

            running = true;
            delay = secondsDelay;
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
            while (running)
            {
                SafeReloadDbSettingProvider();
                Thread.Sleep(delay * 1000);
            }
        }

        private void SafeReloadDbSettingProvider()
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
