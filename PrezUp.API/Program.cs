
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

bool isLogged = false;
if (isLogged)
{
    builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.File(
            "logs/fileupload-.log",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 7
        )
);

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog();
}





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
