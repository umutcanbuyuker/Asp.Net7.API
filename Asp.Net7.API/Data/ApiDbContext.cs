using Asp.Net7.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Asp.Net7.API.Data
{
    public class ApiDbContext : DbContext
    {

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
        public DbSet<ToDo> toDos { get; set; }

    
    }
}
