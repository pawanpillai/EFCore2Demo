using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SomeUI
{
    public class DataHandler
    {
        public DataHandler()
        {
            
        }

        public Samurai MoreQueries()
        {
            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();

            using (var context = new SamuraiContext(options))
            {
                //var name = "Pawan2";
                //return context.Samurais.Where(s => s.Name == name).ToList();
                //return context.Samurais.FirstOrDefault(s => s.Name == name);

                //var searchExpression = "%3";
                //return context.Samurais.FirstOrDefault(s => EF.Functions.Like(s.Name, searchExpression));

                var searchExpression = "Pawan%";
                return context.Samurais.OrderBy(s => s.Id).LastOrDefault(s => EF.Functions.Like(s.Name, searchExpression));
            }


        }

        public List<Samurai> SimpleSamuraiQuery()
        {
            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();

            using (var context = new SamuraiContext(options))
            {
                return context.Samurais.ToList();
            }


        }

        public void InsertMultipleDiffObjects()
        {
            var samurai4 = new Samurai { Name = "Pawan4" };
            var battle = new Battle
            {
                Name = "Battle1",
                StartDate = new DateTime(2018, 12, 02),
                EndDate = new DateTime(2015, 05, 01)
            };

            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();


            using (var context = new SamuraiContext(options))
            {
                context.AddRange(samurai4, battle);
                context.SaveChanges();
            }
        }

        public void InsertMultipleSamurai()
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

        public static void InsertSamurai()
        {
            var samurai = new Samurai { Name = "Pawan" };
            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();


            using (var context = new SamuraiContext(options))
            {
                context.Samurais.Add(samurai);
                context.SaveChanges();
            }
        }

    }
}
