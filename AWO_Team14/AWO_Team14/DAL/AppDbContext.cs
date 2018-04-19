using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using AWO_Team14.Models;

namespace AWO_Team14.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("MyDBConnection") { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Showing> Showings{ get; set; }
        public DbSet<Transaction> Transactions{ get; set; }
        public DbSet<UserTicket> UserTickets { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        //public System.Data.Entity.DbSet<AWO_Team14.Models.Transaction> Transactions { get; set; }

        //public System.Data.Entity.DbSet<AWO_Team14.Models.Showing> Showings { get; set; }
        //public DbSet<User> Users { get; set; }

    }
}