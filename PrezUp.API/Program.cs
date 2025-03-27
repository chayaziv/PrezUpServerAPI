using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PrezUp.API;
using PrezUp.Extesion;
using PrezUp.API.MiddleWares;
using Microsoft.Extensions.Logging;
using Serilog;
using dotenv.net;
using Microsoft.EntityFrameworkCore;
using PrezUp.Data;


// טעינת משתני סביבה מ-.env
DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// עדכון הגדרות ה-JWT ממשתני הסביבה
builder.Configuration["Jwt:Key"] = Environment.GetEnvironmentVariable("JWT_KEY");
builder.Configuration["Jwt:Issuer"] = Environment.GetEnvironmentVariable("JWT_ISSUER");
builder.Configuration["Jwt:Audience"] = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

// עדכון הגדרות ה-AWS ממשתני הסביבה
builder.Configuration["AWS:AccessKey"] = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
builder.Configuration["AWS:SecretKey"] = Environment.GetEnvironmentVariable("AWS_SECRET_KEY");
builder.Configuration["AWS:BucketName"] = Environment.GetEnvironmentVariable("AWS_BUCKET_NAME");

// עדכון Connection String ממשתני הסביבה
builder.Configuration["ConnectionStrings:DefaultConnection"] = Environment.GetEnvironmentVariable("CONNECTION_STRING");

// עדכון כתובת ה-API ממשתני הסביבה
builder.Configuration["AnalysisApi:Url"] = Environment.GetEnvironmentVariable("ANALYSIS_API_URL");

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


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("user"));
    options.AddPolicy("UserOrAdmin", policy => policy.RequireRole("user", "admin"));
});



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
