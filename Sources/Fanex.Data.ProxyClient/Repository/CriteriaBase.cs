namespace Fanex.Data.Repository
{
    public abstract class CriteriaBase : ICriteria
    {
        public abstract string GetSettingKey();

        public abstract bool IsValid();

        public bool IsInvalid()
            => !IsValid();
    }
}