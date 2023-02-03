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

        //Default Ascending
        //ToDo controllerına özel kişiselliştirilebilir crud ve extra metodlarımızı ToDoRepository içerisinde tanımlayabiliriz.
        public override async Task<IEnumerable<ToDo>> All()
        {
            try
            {
                return await _context.toDos.OrderBy(x => x.Id).ToListAsync(); // Default Sıralama Id'ye göre. 'Id-Kategori-Tarih ' göre sıralamalar yapabiliriz
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<ToDo>> AscendingName()
        {
            try
            {
                return await _context.toDos.OrderBy(x => x.Name).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        
    }
}
