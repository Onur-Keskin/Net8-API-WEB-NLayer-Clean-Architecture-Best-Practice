using App.Application.Extensions;
using App.Bus;
using App.Persistance.Extensions;
using CleanApp.API.Extensions;
using CleanApp.API.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithFiltersExt().AddSwaggerGenExt().AddExceptionHandlerExt().AddCachingExt();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddRepositories(builder.Configuration).AddServices(builder.Configuration).AddBusExt(builder.Configuration);

builder.Services.AddJwtExt(builder.Configuration);

// appsettings.json'dan Serilog yapýlandýrmasýný oku
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // appsettings.json'dan yapýlandýrmayý oku
    .CreateLogger();

builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("D:\\Users\\25200340\\OneDrive - Token Finansal Teknolojiler A.S\\Desktop\\Notlarým\\Deneme\\Logs\\log-.txt", rollingInterval: RollingInterval.Day));

var app = builder.Build();

// HTTP isteklerini logla
app.UseSerilogRequestLogging();

app.UseMiddleware<LoggingMiddleware>();

//app.MapGet("/", () => "Hello, .NET 8 with appsettings.json!");

app.UseConfigurePipelineExt();

app.MapControllers();

app.Run();
