using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement
{
    public class Program
    {
        /* At runtime the program looks for the entry point (Main). So it initially starts as a Console application.
         * Main() configures ASP.NET Core and starts it and at that point it becomes an ASP.NET Core web application
         */
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}

/* Note: CreateDefaultBuilder()
 *          - Sets up the web server
 *          - Loading the host and application configutarion from various configuration sources
 *          - Configuring logging
 *          
 * An ASP.NET core applicaiton can be hosted
 *  - InProcess: - CreateDefaultBuilder() calls UseIIS() method and host the app inside of the IIS worker process.
 *               - This type of hosting delivers higher request throughput.
 * 
 *  - OutOfProcess: - 2 Web Servers, Internal(Kestral) and External(IIS, Nginx or Apache)
 *  
 *  Kestral:
 *      - Cross platform web server for ASP.Net Core
 *      - If launced from dotnet run command webserver will be Kestral(dotnet.exe) not IIS
 */
