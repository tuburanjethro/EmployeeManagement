using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

        // Configure method gets called by the runtime. Use this method to configure the 
        // 'HTTP request pipeline'.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                // Middleware: IapplicationBUilder.UseDeveloperExceptionPage()
                app.UseDeveloperExceptionPage();
            }

            // Use() adds a middleware delagate defined in-line to the applications request pipeline
            app.Use(async (context, next) =>
            {
                logger.LogInformation("MW1: Incoming Request");
                await next();
                logger.LogInformation("MW1: Outgoing response");
            });

            // Middleware: IapplicationBUilder.Run() runs the callback function
            // If it is the only thing in the pipeline, all requests will be handled by this single middleware and its only response will be _config["MyKey"]
            // Therefore no matter what the URL is it will respond with _config["MyKey"]. 
            // Even if you want to serve a file from wwwroot, it won't show up.
            app.Use(async (context, next) =>
            {
                //Examples
                {
                    //await context.Response.WriteAsync("Hello World!");
                    //name of the process that is running and hosting our code (IIS or dotnet(Kestral))
                    //await context.Response.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName);

                    //await context.Response.WriteAsync(_config["MyKey"]);
                }

                logger.LogInformation("MW2: Incoming Request");
                await next();
                logger.LogInformation("MW2: Outgoing response");
            });

            //This second middleware will never be hit because of .Run() is a terminal middleware. 
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("MW3: Request handled and response produced");
                logger.LogInformation("MW3: Request handled and response produced");
            });
        }
    }
}

/* MiddleWare in ASP.NET Core:
 * - Has access to both Request and Response
 * - May simply pass the Request to the next Middleware
 * - May handle the Request and short-circuit the pipeline
 * - May process the outgoing Response
 * - Middlewares are executed in the order they are added
 * 
 * Example Pipeline:
 *     ... <----> Logging <----> Static Files <----> MVC <----> ...
 */

/* Configure Request Processing Pipeline:
 *  Everything that happens 'before the next() method' is invoked in each of the middleware components, 
 *  happens as the 'REQUEST' travels from middleware to middleware through the pipeline.
 */