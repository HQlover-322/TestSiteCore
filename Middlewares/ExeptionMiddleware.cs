namespace TEST.Middlewares
{
    public class ExeptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExeptionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch
            {
                await context.Response.WriteAsync("Server Error");
            }
        }
    }
}
