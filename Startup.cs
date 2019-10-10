using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)//Dependency injection, specifically constructor. (shortcut ctor tabx2)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the 'HTTP request pipeline'.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Middleware: IapplicationBUilder.UseDeveloperExceptionPage()
                app.UseDeveloperExceptionPage();
            }

            //Middleware: IapplicationBUilder.Run()
            app.Run(async (context) =>
            {
                //Examples
                {
                //await context.Response.WriteAsync("Hello World!");
                //name of the process that is running and hosting our code (IIS or dotnet(Kestral))
                //await context.Response.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
                }

                await context.Response.WriteAsync(_config["MyKey"]);
            });
        }
    }
}

// MiddleWare in ASP.NET Core
/* - Has access to both Request and Response
 * - May simply pass the Request to the next Middleware
 * - May handle the Request and short-circuit the pipeline
 * - May process the outgoing Response
 * - Middlewares are executed in the order they are added
 * 
 * Example Pipeline:
 *     ... <----> Logging <----> Static Files <----> MVC <----> ...
 */
