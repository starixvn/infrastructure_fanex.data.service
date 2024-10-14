namespace Fanex.Data.Vault
{
    public class DatabaseCredential
    {
        public DatabaseCredential(string key)
        {
            Key = key;
        }

        public string Key { get; }

        public string IP { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as DatabaseCredential;

            return item != null
                   && Key == item.Key
                   && IP == item.IP
                   && Username == item.Username
                   && Password == item.Password;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}
