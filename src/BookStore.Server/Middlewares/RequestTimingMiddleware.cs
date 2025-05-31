using System.Diagnostics;

namespace BookStore.Server.Middlewares
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopWatch = new Stopwatch();

            try
            {
                stopWatch.Start();
                await _next(context);
            }
            finally
            {
                stopWatch.Stop();

                var elapsedMilliseconds = stopWatch.ElapsedMilliseconds;

                var method = context.Request.Method;
                var path = context.Request.Path;

                // ANSI escape codes for coloring
                var logWithColor = $"\u001b[1;32;40m{method} {path} request took \u001b[1;34;40m{elapsedMilliseconds}ms\u001b[0m to complete";

                // Write colored message directly to console
                Console.WriteLine(logWithColor);

                // Also log through logger (without color)
                _logger.LogInformation("{RequestMethod} {RequestPath} request took {EllapsedMilliseconds}ms to complete",
                    method, path, elapsedMilliseconds);
            }
        }
    }
}
