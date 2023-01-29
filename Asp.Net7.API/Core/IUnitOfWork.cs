namespace Asp.Net7.API.Core
{
    public interface IUnitOfWork 
    {
        IToDoRepository ToDos { get; }
        Task CompleteAsync();
    }
}
