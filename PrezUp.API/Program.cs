
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PrezUp.API;
using PrezUp.Extesion;
using PrezUp.API.MiddleWares;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.ServieDependencyInjector(builder.Configuration);
builder.Services.AddCorsPolicy();
builder.Services.AddAutoMapper(typeof(MappingPostEntity));


builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)  // קריאה מה-`appsettings.json`
        .WriteTo.File("logs/fileupload.log", rollingInterval: RollingInterval.Day) // הגדרת קובץ הלוג
);

// הגדרת Serilog כמנהל הלוגים
builder.Logging.ClearProviders();  // נקה את המפיקים ברירת המחדל
builder.Logging.AddSerilog();  // הוסף את Serilog




var app = builder.Build();




if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/presentation/analyze-audio"), builder =>
{
    builder.UseMiddleware<FileUploadRateLimitMiddleware>();
});

app.MapControllers();

app.Run();
