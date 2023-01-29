using Asp.Net7.API.Data;
using Asp.Net7.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Asp.Net7.API.Core.Repositories
{
    public class ToDoRepository : GenericRepository<ToDo>, IToDoRepository
    {
        public ToDoRepository(ApiDbContext context, ILogger logger) : base(context, logger)
        {

        }

        public override async Task<IEnumerable<ToDo>> All()
        {
            try
            {
                return await _context.toDos.Where(x => x.Id < 10).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public override async Task<ToDo> GetById(int id)
        {
            try
            {
                return await _context.toDos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
