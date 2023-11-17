namespace Fanex.Data.Repository.Tests
{
    public class DynamicRepositoryFixture
    {
        public IDynamicRepository Repository { get; } = new DynamicRepository();
    }
}