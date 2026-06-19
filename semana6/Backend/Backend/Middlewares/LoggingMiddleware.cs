namespace Backend.Middlewares
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
            var method = context.Request.Method;
            var path = context.Request.Path;
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "IP desconocida";

            string body = string.Empty;
            if (context.Request.ContentLength > 0 &&
                (method == "POST" || method == "PUT" || method == "PATCH"))
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            _logger.LogWarning("➡ [{IP}] {Method} {Path} | Body: {Body}",
                ip, method, path, string.IsNullOrEmpty(body) ? "(sin body)" : body);

            await _next(context);
        }
    }
}
