namespace Fanex.Data.Repository
{
    public interface ICriteria
    {
        string GetSettingKey();

        bool IsValid();
    }
}