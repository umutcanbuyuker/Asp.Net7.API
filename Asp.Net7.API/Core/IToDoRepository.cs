using Asp.Net7.API.Models;

namespace Asp.Net7.API.Core
{
    public interface IToDoRepository:IGenericRepository<ToDo> 
    {
        Task<IEnumerable<ToDo>> AscendingName();
    }
}
