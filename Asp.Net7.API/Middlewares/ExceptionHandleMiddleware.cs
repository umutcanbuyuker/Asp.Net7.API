namespace Asp.Net7.API.Middlewares
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandleMiddleware(RequestDelegate Next) //next dendiği zaman bir sonraki middleware'e işlemi delegate edebilmesi için RequestDelegate tipini kullanıyoruz
        {
            _next = Next;
        }
        //Dışardan bir istek geldiğinde controller'dan önce middleware gelerek invoke metotu tetiklenecek.
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception)
            {
                throw;
                // Burada exception error handling yapan custom bir metot yazacağız.
            }
        }
    }
}

