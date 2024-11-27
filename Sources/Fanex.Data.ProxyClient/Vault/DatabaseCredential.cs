namespace Fanex.Data.Vault;

public class DatabaseCredential(string key)
{
    public string Key { get; } = key;

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