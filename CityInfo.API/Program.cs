using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

//FROM PLURALSIGHT COURSE Building Your First API with ASP.NET Core

namespace CityInfo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args) //this does a lot, including set up logging to Console and Debug
                //.ConfigureLogging((hostingContext, logging) =>
                //{
                //    logging.AddConsole();
                //    logging.AddDebug();
                //    logging.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider()); //use AddProvider to add third party loggers
                //    logging.AddNLog(); //many third parties include an Add method that can be used instead of calling AddProvider
                //})
                .UseStartup<Startup>()
                .Build();
    }
}
