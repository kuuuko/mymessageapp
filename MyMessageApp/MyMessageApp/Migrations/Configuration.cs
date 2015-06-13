namespace MyMessageApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MyMessageApp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MyMessageApp.Models.MessageDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MyMessageApp.Models.MessageDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Messages.AddOrUpdate( 
                p => p.text, 
                new Message 
                {
                   text = "Prva poruka",
                   level = 0,
                   order = 0,
                   parentID = 0,
                },
                new Message 
                {
                   text = "Druga poruka",
                   level = 0,
                   order = 2,
                   parentID = 0,
                },
                new Message 
                {
                   text = "Odgovor na Prvu poruku",
                   level = 1,
                   order = 1,
                   parentID = 1,
                });              
        }
    }
}
