using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Linq;
using System.Collections.Generic;

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


            //InsertSamurai();
            //InsertMultipleSamurai();
            //InsertMultipleDiffObjects();
            var samurais = SimpleSamuraiQuery();

            app.Run(async (context) =>
            {
                //await context.Response.WriteAsync("Hello World!");
                foreach (var samurai in samurais)
                {   
                    await context.Response.WriteAsync(samurai.Name + "\n");
                }
            });
        }

        private List<Samurai> SimpleSamuraiQuery()
        {
            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();

            using (var context = new SamuraiContext(options))
            {
               return context.Samurais.ToList();
            }


        }

        private void InsertMultipleDiffObjects()
        {
            var samurai4 = new Samurai { Name = "Pawan4" };
            var battle = new Battle { 
                Name = "Battle1",
                StartDate = new DateTime(2018, 12,02),
                EndDate = new DateTime(2015, 05, 01)
            };

            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();


            using (var context = new SamuraiContext(options))
            {
                context.AddRange(samurai4, battle);
                context.SaveChanges();
            }
        }

        private void InsertMultipleSamurai()
        {
            var samurai2 = new Samurai { Name = "Pawan2" };
            var samurai3 = new Samurai { Name = "Pawan3" };

            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();


            using (var context = new SamuraiContext(options))
            {
                context.Samurais.AddRange(samurai2, samurai3);
                context.SaveChanges();
            }
        }

        private static void InsertSamurai()
        {
            var samurai = new Samurai { Name = "Pawan"};
            DbContextOptions <SamuraiContext> options = new DbContextOptions<SamuraiContext>();


            using (var context = new SamuraiContext(options)){
                context.Samurais.Add(samurai);
                context.SaveChanges();
            }
        }
    }
}
