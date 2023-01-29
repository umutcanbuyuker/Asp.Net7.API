using Asp.Net7.API.Core;
using Asp.Net7.API.Core.Repositories;

namespace Asp.Net7.API.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApiDbContext _context;
        private readonly ILogger _logger;
        public IToDoRepository ToDos { get; private set; }
        public UnitOfWork(ApiDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            var _logger = loggerFactory.CreateLogger(categoryName:"logs");
            ToDos = new ToDoRepository(_context,_logger);
        }

        public async Task CompleteAsync()
        {
           await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
