using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SamuraiApp.Data;

namespace SomeUI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        //console logger
        public static readonly LoggerFactory MyConsoleLoggerFactory
            = new LoggerFactory(new[] {
                new ConsoleLoggerProvider((category, level)
                    => category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information, true )});

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SamuraiContext>(
                options => options
                .UseLoggerFactory(MyConsoleLoggerFactory)
                .UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            DataHandler dataHandler = new DataHandler();

            //dataHandler.InsertSamurai();
            //dataHandler.InsertMultipleSamurai();
            //dataHandler.InsertMultipleDiffObjects();
            //var samurais = dataHandler.SimpleSamuraiQuery();
            //var samurais = dataHandler.MoreQueries();
            //dataHandler.RetrieveAndUpdateSamurai();
            //dataHandler.MultipleDatabaseChanges();
            //dataHandler.QueryAndUpdateBattle_Disconnected();
            //dataHandler.DeleteWhileTracked();
            //dataHandler.InsertRelatedData();
            //dataHandler.InsertRelatedDataWhenNotTracked();
            dataHandler.EagerLoadSamuraiWithQuotes();


            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");

                //foreach (var samurai in samurais)
                //{   
                //    await context.Response.WriteAsync(samurai.Name + "\n");
                //}

                //await context.Response.WriteAsync(samurais.Name + "\n");
            });
        }


    }
}
