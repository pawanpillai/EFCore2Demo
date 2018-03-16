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

        public void ModifyingRelatedDataWhenNotTracked()
        {
            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();

            var context = new SamuraiContext(options);
            var samurai = context.Samurais.Select(s => new { s.Quotes }).LastOrDefault();
            var quote = samurai.Quotes[0];
            quote.Text += "-- more suffix";

            var newContext = new SamuraiContext(options);
            newContext.Entry(quote).State = EntityState.Modified;
            newContext.SaveChanges();

        }


        public void ModifyingRelatedDataWhenTracked()
        {
            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();

            var context = new SamuraiContext(options);

            var samurai = context.Samurais.Select(s => new { s.Quotes }).LastOrDefault();

            samurai.Quotes[0].Text += " - Suffix.";

            context.SaveChanges();

        }


        public void FilteringWithRelatedData()
        {
            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();

            using (var context = new SamuraiContext(options))
            {
                var samuraiWithQuotes = context.Samurais
                                               .Where(s => s.Quotes.Any(q => q.Text.Contains("fine")))
                                               .ToList();
            }
        }


        public void ProjectSamuraiWithQuotes()
        {
            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();

            using (var context = new SamuraiContext(options))
            {
                var samuraiWithQuotes = context.Samurais.Select(s => new { s.Id, s.Name, s.Quotes })
                                               .ToList();
            }
        }

        public void EagerLoadSamuraiWithQuotes()
        {
            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();

            using (var context = new SamuraiContext(options))
            {
                var samuraiWithQuotes = context.Samurais.Where(s => s.Name == "Pawan50")
                                               .Include(s => s.Quotes)
                                               .FirstOrDefault();
            }
        }

        public void InsertRelatedDataWhenNotTracked()
        {
            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();

            var context = new SamuraiContext(options);

            var samurai = context.Samurais.LastOrDefault();

            using(var newContext = new SamuraiContext(options)){
                var quote = new Quote { 
                    Text = "Life goes on !",
                    SamuraiId = samurai.Id
                };


                newContext.Quotes.Add(quote);
                newContext.SaveChanges();
            }
        }

        public void InsertRelatedData()
        {
            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();

            var context = new SamuraiContext(options);

            var samurai = new Samurai { 
                Name = "Pawan50",
                Quotes = new List<Quote>{
                    new Quote { Text = "Everything will be fine !" }
                }
            };

            context.Samurais.Add(samurai);
            context.SaveChanges();

        }

        public void DeleteWhileTracked()
        {
            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();

            var context = new SamuraiContext(options);
            var samurai = context.Samurais.LastOrDefault();

            context.Samurais.Remove(samurai);
            context.SaveChanges();

            //alternate... call a stored procedure
            //context.Database.ExecuteSqlCommand("exec SomeStoredProcedure {0}", samurai.Id);

        }

        public void QueryAndUpdateBattle_Disconnected()
        {
            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();

            var context = new SamuraiContext(options);
            var battle = context.Battles.FirstOrDefault();
            battle.EndDate = new DateTime(2018, 02, 28);

            using(var newContext = new SamuraiContext(options)){
                newContext.Battles.Update(battle);
                newContext.SaveChanges();
            }

        }

        public void MultipleDatabaseChanges()
        {
            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();

            using (var context = new SamuraiContext(options))
            {

                var samurais = context.Samurais.ToList();
                foreach(var s in samurais){
                    s.Name += "0";
                }

                context.Samurais.Add(new Samurai{ Name = "Pawan50"});

                context.SaveChanges();
            }
        }

        public void RetrieveAndUpdateSamurai()
        {
            DbContextOptions<SamuraiContext> options = new DbContextOptions<SamuraiContext>();

            using (var context = new SamuraiContext(options))
            {

                var samurai = context.Samurais.FirstOrDefault();
                //samurai.Name += "0";
                samurai.Name = samurai.Name.Replace("0", "1");
                context.SaveChanges();
            }
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
