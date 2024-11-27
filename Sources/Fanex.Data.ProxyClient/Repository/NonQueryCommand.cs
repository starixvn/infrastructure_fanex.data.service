namespace Fanex.Data.Repository;

public interface INonQueryCommand : ICriteria;

public abstract class NonQueryCommand : CriteriaBase, INonQueryCommand;