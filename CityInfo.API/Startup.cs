using CityInfo.API.Entities;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace CityInfo.API
{
    public class Startup
    {
        //app config
        public static IConfiguration Configuration { get; private set; }

        //constructor is only needed to load appSettings into Configuration (replaces app.config)
        public Startup(IConfiguration configuration)
        {
            //this is set up by CreateDefaultBuilder in Program.Main
            //uses appSettings.json
            //file is required and reloads on change
            //can create environment-specific files, just like transforms (the environment variable in the project properties)
            //also loads environment variables
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //configure input/output formats (input/output default to json; this code allows xml output)
            //.AddMvcOptions(o => o.OutputFormatters.Add(
            //    new XmlDataContractSerializerOutputFormatter()))

            //configure json to return names as defined in dto instead of changing the casing
            //.AddJsonOptions(o => {
            //    if (o.SerializerSettings.ContractResolver != null)
            //    {
            //        var castedResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
            //        castedResolver.NamingStrategy = null;
            //    }
            //});

            //add our custom service to the DI container - methods control lifetime (AddTransient, AddScoped, AddSingleton)
            //in classes that use this service instantiate this service in the class constructor
            services.AddTransient<IMailService,LocalMailService>();

            //register EF for context - connection string should be in an environment variable
            var connectionString = "Server=(local);Database=CityInfoDB;Trusted_Connection=true;";
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString));

            //add repository class to the DI container
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();

            //add Mongo repository to the DI container (just like any other service)
            //Mongo example from https://www.codeproject.com/Articles/1151842/Using-MongoDB-NET-Driver-with-NET-Core-WebAPI
            services.AddTransient<IMongoRepository, MongoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, CityInfoContext cityInfoContext)
        {
            //THE ORDER MIDDLEWARE IS ADDED IS IMPORTANT

            //First set up exception handling
            //controlled by project-debug-environment variables
            if (env.IsDevelopment())
            {
                //show yellow page of death with stack trace
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //don't show yellow page of death - log exception and redirect to error page
                app.UseExceptionHandler();
            }

            //seed database if needed
            cityInfoContext.EnsureSeedDataForContext();

            //Next return text message for bad status codes like 404
            app.UseStatusCodePages();

            //Next add MVC (centralized routing)
            //app.UseMvc(config => {
            //    config.MapRoute(
            //        name:"Default",
            //        template: "{controller}/{action}/{id?}",
            //        defaults: new { controller = "Home", action = "Index" });
            //});

            //for attribute routing just use this and add attributes in controllers (recommended)
            app.UseMvc();

            //Finally run the app
            //if a route was found then got controller result but if a route was not found it fell through and returned Hello World - there was no error
            app.Run(async (context) =>
            {
                //throw new System.Exception("test");
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
