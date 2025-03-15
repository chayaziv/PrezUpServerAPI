namespace PrezUp.API.MiddleWares
{
    public class FileUploadRateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<FileUploadRateLimitMiddleware> _logger;
        private static readonly Dictionary<string, DateTime> LastUploadTimes = new();

        public FileUploadRateLimitMiddleware(RequestDelegate next, ILogger<FileUploadRateLimitMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User?.Identity?.Name; 

            if (string.IsNullOrEmpty(userId))
            {
               _logger.LogWarning("User not authenticated.");
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("User is not authenticated.");
                return;
            }

            var now = DateTime.UtcNow;

           
            if (LastUploadTimes.ContainsKey(userId) && (now - LastUploadTimes[userId]).TotalMinutes < 1)
            {
                _logger.LogWarning("User {UserId} is trying to upload a file too soon. Please wait for 1 minutes.", userId);
                context.Response.StatusCode = 429; // Too Many Requests
                await context.Response.WriteAsync("You can only upload files every 1 minutes.");
                return;
            }

           
            LastUploadTimes[userId] = now;

            await _next(context); 
        }
    }

}
