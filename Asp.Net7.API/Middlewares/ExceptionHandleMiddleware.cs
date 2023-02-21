using Newtonsoft.Json;
using System.Net;

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
            catch (Exception ex)
            {
                // Burada exception error handling yapan custom bir metot yazıyoruz.
                await HandleException(httpContext, ex);
            }
        }

        private Task HandleException(HttpContext context, Exception ex)
        {
            string message = "[Error] HTTP" + context.Request.Method+ " - " + context.Response.StatusCode + "Error Message" + ex.Message + " in ";

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = JsonConvert.SerializeObject(new { error = ex.Message }, Formatting.None);
            return context.Response.WriteAsync(result);
        }
    }
}

