using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


// import the Models (representing structure of tables in database)
using Community.Core.Models; 

namespace Community.Data.Repositories
{
    // The Context is How EntityFramework communicates with the database
    // We define DbSet properties for each table in the database
    public class DatabaseContext :DbContext
    {
         // authentication store
        public DbSet<User> Users { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Post> Posts { get; set;}
        public DbSet<Comment> Comments {get; set;}
        public DbSet<NewsArticle> NewsArticles { get; set;}
        public DbSet<Venue> Venues { get; set;}
        public DbSet<Event> Events { get; set;}
        public DbSet<Issue> Issues {get; set;}
        public DbSet<Location> Locations {get; set;}


        //***************ORGINIAL METHOD OF DATABASE CONNECTION USING MYSQL***********************
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //    optionsBuilder                  
        //         .UseMySQL("server=127.0.0.1; port=3306; database=cahir_com810ddb; user=root; password=com810")
        //         .LogTo(Console.WriteLine, LogLevel.Information) // remove in production
        //         .EnableSensitiveDataLogging()                   // remove in production
        //         ;
        // }
        //public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder               
                /** use simple logging to log the sql commands issued by entityframework **/ 
                //.LogTo(Console.WriteLine, LogLevel.Information)
                //.EnableSensitiveDataLogging()
                .UseSqlite("Filename=data.db");
        }

        // Convenience method to recreate the database thus ensuring  the new database takes 
        // account of any changes to the Models or DatabaseContext
        public void Initialise()
        {
           Database.EnsureDeleted();
           Database.EnsureCreated();
        }

    }
}
