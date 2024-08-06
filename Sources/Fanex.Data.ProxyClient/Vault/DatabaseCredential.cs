namespace Fanex.Data.Vault
{
    public class DatabaseCredential
    {
        private readonly string key;

        public DatabaseCredential(string key)
        {
            this.key = key;
        }

        public string Key => key;

        public string IP { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as DatabaseCredential;

            return item != null
                   && key == item.Key
                   && IP == item.IP
                   && Username == item.Username
                   && Password == item.Password;
        }

        public override int GetHashCode()
        {
            return key.GetHashCode();
        }
    }
}
