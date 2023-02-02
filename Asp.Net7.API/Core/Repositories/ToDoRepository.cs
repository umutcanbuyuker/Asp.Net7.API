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

        //ToDo controllerına özel kişiselliştirilebilir crud metodlarımızı burada tanımlayabiliriz.
        // Sıralama işlemini orderBy ile düzenleyebiliriz
        public override async Task<IEnumerable<ToDo>> All()
        {
            try
            {
                return await _context.toDos.OrderBy(x => x.Id).ToListAsync(); // Custom Sıralama Id-Kategori-Tarih ' göre sıralamalar yapabiliriz
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
   
    }
}
