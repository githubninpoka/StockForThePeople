using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Serilog;
using StockForThePeople.Data;
using StockForThePeople.ExternalData;
using StockForThePeople.ExternalData.DTO;
using StockForThePeople.InternalData;
using StockForThePeople.WebApiExecuter;

namespace StockForThePeople.WebApi;

// I need a service that will update ticker information once per day.
// I am not sure where it is supposed to live,
// but for now I will just reference the service
// inside the API project and not update automatically.
// There are probably better ways.
public class Program
{
    public static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        Log.Logger = new LoggerConfiguration() // needs Serilog.AspNetCore Nuget package
               .ReadFrom.Configuration(configuration) // needs Serilog.Settings.Configuration Nuget package
               .CreateLogger();

        Serilog.Debugging.SelfLog.Enable(Console.Error);

        builder.Host.UseSerilog();
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();
        builder.Services.AddSerilog();

        builder.Services.AddHttpClient();

        builder.Services.AddCors(policy =>
        {
            policy.AddPolicy("allowAll", builder =>
            builder.WithOrigins("*")
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyOrigin());
        });
        builder.Services.Configure<ExternalDataConfigurationOptions>(
            builder.Configuration.GetSection(key: "ExternalData"));
        builder.Services.AddTransient<IWebApiExecuter, GenericWebApiExecuter>();
        builder.Services.AddScoped<IExternalDataService, ExternalDataService>();
        builder.Services.AddScoped<IInternalDataService, InternalDataService>();

        builder.Services.AddDbContext<IStockForThePeopleContext, StockForThePeopleSqliteContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("StockForThePeopleDb"));
        });

        builder.Services.AddHostedService<UpdateExternalDataBackgroundTask>();


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "The API for Stock For The People", Version = "v1" });
        });

        builder.Services.AddOutputCache(options =>
        {
            options.AddBasePolicy(builder =>
            {
                builder.Expire(TimeSpan.FromSeconds(10));
            });
            options.AddPolicy("Expire30", builder =>
            {
                builder.Expire(TimeSpan.FromSeconds(30));
                builder.Tag("TagHandleForExpire30Policy");
            });
            options.AddPolicy("Expire300", builder =>
            {
                builder.Expire(TimeSpan.FromSeconds(300));
                builder.SetLocking(true);
                builder.Tag("TagHandleForExpire300Policy");
            });
        });



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseCors("allowAll");

        app.UseOutputCache();

        app.MapControllers();

        //app.RunAsync();
        app.Run();

    }
}
