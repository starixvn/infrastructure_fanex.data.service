using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Fanex.Data.ProxyClient.Tests")]
namespace Fanex.Data.Vault
{
    using System;
    using System.Threading;

    internal class VaultPollingService
    {
        private Thread pollingThread;

        private bool running;

        private int intervalInSeconds = 5 * 60;

        private readonly VaultService vaultService;

        internal VaultPollingService(VaultProvider provider)
        {
            vaultService = new VaultServiceImp(provider);
        }

        public Exception LatestException { get; private set; }

        public void Start(int secondsDelay)
        {
            Stop();

            running = true;
            intervalInSeconds = secondsDelay;
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
                Thread.Sleep(intervalInSeconds * 1000);
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
