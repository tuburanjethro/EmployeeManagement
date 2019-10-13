using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
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
            services.AddMvc();

            /* If a controller requests this IRepository service, then create an instance of MockEmployeeRepository 
             */
            services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();
        }

        // Configure method gets called by the runtime. Use this method to configure the 
        // 'HTTP request pipeline'.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World");
            });
        }
    }
}

/* Previous Examples:
 *  await context.Response.WriteAsync("Hello World!");
 *  name of the process that is running and hosting our code(IIS or dotnet(Kestral))
 *  await context.Response.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
 *  await context.Response.WriteAsync(_config["MyKey"]);
 */

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

/* Middleware: IapplicationBUilder.Run() runs the callback function
 *   If it is the only thing in the pipeline, all requests will be handled by this single middleware and its only response will be _config["MyKey"]
 *   Therefore no matter what the URL is it will respond with _config["MyKey"]. 
 *   Even if you want to serve a file from wwwroot, it won't show up.
 */

/* Configure Request Processing Pipeline:
 *  - Everything that happens 'before the next() method' is invoked in each of the middleware components, 
 *    happens as the 'REQUEST' travels from middleware to middleware through the pipeline.
 *  - Note: Use() adds a middleware delagate defined in-line to the applications request pipeline
 */

/* Static Files:
 *   - By default an ASP.NET COre application 'will not serve static files'
 *   - The default directory for static files is the wwwroot
 *   - To serve static files 'UseStaticFiles()' middleware is required
 *   
 *   - To serve a default file 'UseDefaultFiles()' middleware is required
 *   - Default files include index.htm(l) or default.html(l)
 *   - UseDefaultFiles() 'must be registered before' UseStaticFiles()
 *   
 *   - UseFileServer() 'combines the functionality' of UseStaticFiles, UseDefaultFiles and UseDirectoryBrowser middleware
 *   
 *   - Each one can be overloaded to add default options.
 *      - E.g. 
 *          FileServerOptions fileServerOptions = new FileServerOptions();
 *          fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
 *          fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("foo.html");
 *      - Then app.UseFileServer(fileServerOptions);
 */

/* Static Files Examples: 
 * 
 * DefaultFileOptions()
 * - Changes the default page from default.html/index.html to whatever you want it to be.
 * - E.g. DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
 *        defaultFilesOptions.DefaultFileNames.Clear();
 *        defaultFilesOptions.DefaultFileNames.Add("foo.html");
 *        app.UseDefaultFiles()
 *  - Changes the request path to point to default.html (aka. index.html) but DOES NOT SERVE.
 *  - If it is after the UseStaticFiles() path is changed but doesn't serve it to the user
 *  - Contains 2 overloads (go to definition)
 *    + app.UseDefaultFiles(defaultFilesOptions);
 *    + Now serves whatever defaultFileOptions are
 *  
 * UseStaticFiles();
 * - Allows you to serve static files
 */

/* Developer Exception Page:
 *   - To enable plug in 'UseDeveloperExceptionPage' Middleware in the pipeline
 *   - Must be be plugged in the pipline 'as early as possible'
 *   - Contains 'Stack Trace, Query String, Cookies and HTTP headers'
 *   - For customizing use 'DeveloperExceptionPageOptions'
 *   - E.g.
 *          //Developer Exception page shows a detailed error for developers. Instead of page not found.
 *          DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions()
 *          {
 *              //Shows the area of code where the exception is thrown.
 *              SourceCodeLineCount = 10
 *          };
 *          app.UseDeveloperExceptionPage(developerExceptionPageOptions);
 *          
 *    - Developer Exception page shows a detailed error for developers. Instead of page not found.
 *          DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions()
 *          {
 *              //Shows the area of code where the exception is thrown.
 *              SourceCodeLineCount = 10
 *          };
 *          app.UseDeveloperExceptionPage(developerExceptionPageOptions);
 */

/* Environment Variables:
 *     | Development | Staging | Production |
 *      
 *   -  ASPNETCORE_ENVIRONMENT variable sets the 'Runtime Environment'
 *   -  On development machine we set it in 'launchsettings.json' file
 *   -  On Staging or Production server we set in the 'operating system'
 *   -  Use 'IHostingEnvironment' service to access the runtime environment
 *   -  Runtime environement defaults to 'Production' if not set explicitly
 *   -  In addition to 'standard environments' (Development, Staging, Production), 'custom environments' are supported
 */

/* MVC:
 * An architechtural design pattern for implementing User Interface Layer of an application
 * 
 * Model: Set of classes that represent data + the logic to manage that data (Employee and EmployeeRepository)
 * View: Contains the display logic to present the Model data provided to it by the Controller
 * Controller: Handles the http request, work with the model, and selects a view to render that model
 */

/* Setup MVC in ASP.NET Core
 *   - Step 1: Add the MVC Services to the Dependency Injection Container
 *          public void ConfigureServices(IServiceCollection services)
 *          {
 *              services.AddMvc();
 *          }
 *        
 *   - Step 2: Add MVC middleware to the Request Pipline
 *          app.UseMvcWithDefaultRoute();
 */

/* AddMvc() vs AddMvcCore()
 *   - AddMvcCore() method only adds the core MVC services
 *   - AddMvc() method adds all the required MVC services (incl. Razor)
 *   - AddMvc() method calls AddMvcCore() method internally
 *   
 * Note: .NET Core 3.0 has split this up into 
 *      - services.AddControllers()
 *          - Adds support for controllers and API-related features - but not views or pages.
 *          - Used by the API template
 *      - services.AddControllersWithViews()
 *          - Adds support for controllers, API-related features, and views - but not pages.
 *          - Used by the Web Application (MVC) template.
 *      - services.AddRazorPages()
 *          - Adds support for Razor Pages and minimal controller support.
 *          - Used by the Web Application template.
 *      - services.AddMvc() => services.AddControllers();
 *                             services.AddRazorPages();
 */

/* Dependency Injection: constructor
 *   - To Register with Dependency Injection Container
 *          - AddSingleton()
 *              - Only created one time per application and that instance is used throughout the application lifetime.
 *          - AddTrandient()
 *              - New instance is created EACH TIME it is requested
 *          - AddScoped()
 *              - Instance created once per scope.
 *              - E.g. In an MVC application it creates one instance for each http request but used the same instance within that same request
 *   - If a controller requests this IRepository service, then create an instance of MockEmployeeRepository 
 *          - services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();
 *      
 *   - Why not create a new instance? 
 *          - If use the 'new' implementation because it loosely couples the code
 *          - Imagine having hundreds of controllers
 *          
 *   - Benefits of DI
 *          - Loose Coupling
 *          - Easy to unit test
 */
