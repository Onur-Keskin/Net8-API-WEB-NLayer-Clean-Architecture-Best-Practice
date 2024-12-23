namespace CleanApp.API.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Gelen istek bilgilerini logla
            _logger.LogInformation("HTTP {Method} {Path} isteği alındı.", context.Request.Method, context.Request.Path);

            // İşlemi bir sonraki middleware'e ilet
            await _next(context);

            // Giden yanıt bilgilerini logla
            _logger.LogInformation("HTTP {Method} {Path} isteği yanıtlandı. Status Code: {StatusCode}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode);
        }
    }

}
