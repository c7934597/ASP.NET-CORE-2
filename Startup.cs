using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace MyWebsite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //Program.Output("Startup Constructor - Called");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<ISampleTransient, Sample>();
            services.AddScoped<ISampleScoped, Sample>();
            services.AddSingleton<ISampleSingleton, Sample>();
            // Singleton 也可以用以下方法註冊
            // services.AddSingleton<ISampleSingleton>(new Sample());

            //Program.Output("Startup.ConfigureServices - Called");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            /*appLifetime.ApplicationStarted.Register(() =>
            {
                Program.Output("ApplicationLifetime - Started");
            });

            appLifetime.ApplicationStopping.Register(() =>
            {
                Program.Output("ApplicationLifetime - Stopping");
            });

            appLifetime.ApplicationStopped.Register(() =>
            {
                Thread.Sleep(5 * 1000);
                Program.Output("ApplicationLifetime - Stopped");
            });

            //app.UseMiddleware<FirstMiddleware>();
            //app.UseFirstMiddleware();

            app.Use(async (context, next) => 
            {
                await context.Response.WriteAsync("First Middleware in. \r\n");
                await next.Invoke();
                await context.Response.WriteAsync("First Middleware out. \r\n");
            });

            app.Map("/second", mapApp =>
            {
                mapApp.Use(async (context, next) => 
                {
                    await context.Response.WriteAsync("Second Middleware in. \r\n");

                    //// 水管阻塞，封包不往後送
                    //var condition = false;
                    //if(condition) {
                    //    await next.Invoke();
                    //}
                    
                    await next.Invoke();
                    await context.Response.WriteAsync("Second Middleware out. \r\n");
                });
                mapApp.Run(async context =>
                {
                    await context.Response.WriteAsync("Second. \r\n");
                });
            });

            app.Use(async (context, next) => 
            {
                await context.Response.WriteAsync("Third Middleware in. \r\n");
                await next.Invoke();
                await context.Response.WriteAsync("Third Middleware out. \r\n");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });

            //// For trigger stop WebHost (測試生命週期)
            //var thread = new Thread(new ThreadStart(() =>
            //{
            //    Thread.Sleep(5 * 1000);
            //    Program.Output("Trigger stop WebHost");
            //    appLifetime.StopApplication();
            //}));
            //thread.Start();*/

            //Program.Output("Startup.Configure - Called");
        }
    }
}
