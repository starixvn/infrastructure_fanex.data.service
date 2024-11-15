namespace Fanex.Data.Repository;

public interface ICriteria
{
    string GetSettingKey();

    bool IsValid();
}

public abstract class CriteriaBase : ICriteria
{
    public abstract string GetSettingKey();

    public abstract bool IsValid();

    public bool IsInvalid()
        => !IsValid();
}