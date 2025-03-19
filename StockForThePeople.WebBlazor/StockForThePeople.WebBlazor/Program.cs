using StockForThePeople.WebApiExecuter;
using StockForThePeople.WebBlazor.Client;
using StockForThePeople.WebBlazor.Client.Pages;
using StockForThePeople.WebBlazor.Components;

namespace StockForThePeople.WebBlazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpClient();
            builder.Services.AddTransient<IWebApiExecuter, GenericWebApiExecuter>();
            StockForThePeopleSettings stockForThePeopleSettings = new StockForThePeopleSettings()
            {
                BaseUrl = builder.Configuration.GetValue<string>("StockForThePeopleApi:BaseUrl"),
                AssetListUri = builder.Configuration.GetValue<string>("StockForThePeopleApi:AssetListUri"),
                MarketByTickerUri = builder.Configuration.GetValue<string>("StockForThePeopleApi:MarketByTickerUri")
            };
            builder.Services.AddSingleton(stockForThePeopleSettings);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(BlazorComponentsLibrary.ForUsingInBlazor.ForUsingInBlazor).Assembly)
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);
                

            app.Run();
        }
    }
}
