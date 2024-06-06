namespace Fanex.Data.Vault
{
    public class VaultOptions
    {
        public string Address { get; set; }

        public string Role { get; set; }

        public string SecretId { get; set; }

        public string MountPath { get; set; }

        public bool Enable { get; set; } = false;

        public int MinutesPolling { get; set; } = 5;
    }
}
