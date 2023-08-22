using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { set; get; }

        /////////// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {

                app.UseExceptionHandler("/Error");
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("secrets/appsettings.secrets.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            Console.WriteLine("App starting...");

            app.UseRouting();

            app.UseAuthorization();

            app.Run(async (context) =>
            {
                var message = $"Host: {Environment.MachineName}\n" +
                    $"EnvironmentName: {env.EnvironmentName}\n" +
                    $"Secret value: {(env.EnvironmentName == "Production" ? Environment.GetEnvironmentVariable("ConnectionStrings__DBGOEDU_Mix") : Configuration["ConnectionString:DBGOEDU_Mix"])}\n" +
                    $"Datetime now: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\n" +
                    $"Utc now: {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}";
                await context.Response.WriteAsync(message);
            });

            Console.WriteLine("Starting done!");
            TimeZoneInfo localZone = TimeZoneInfo.Local;
            Console.WriteLine("Local Time Zone ID: {0}", localZone.Id);
            Console.WriteLine("   Display Name is: {0}.", localZone.DisplayName);
            Console.WriteLine("   Standard name is: {0}.", localZone.StandardName);
            Console.WriteLine("   Daylight saving name is: {0}.", localZone.DaylightName);

            DateTime startTime = TimeZoneInfo.ConvertTimeFromUtc(new System.DateTime(1970, 1, 1), TimeZoneInfo.Utc);

            Console.WriteLine("startTime: {0}.", startTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
