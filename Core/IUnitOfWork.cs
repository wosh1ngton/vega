namespace vega.Core
{

    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}