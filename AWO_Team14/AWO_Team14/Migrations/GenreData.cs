//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using AWO_Team14.Models;
//using AWO_Team14.DAL;
//using System.Data.Entity.Migrations;

//namespace AWO_Team14.Migrations
//{
//    public class GenreData
//    {
//        public void SeedGenres(AppDbContext db)
//        {
//            Genre g1 = new Genre();
//            g1.GenreName = "Musical";
//            db.Genres.AddOrUpdate(g => g.GenreName, g1);
//            db.SaveChanges();

//            Genre g2 = new Genre();
//            g2.GenreName = "Comedy";
//            db.Genres.AddOrUpdate(g => g.GenreName, g2);
//            db.SaveChanges();

//            Genre g3 = new Genre();
//            g3.GenreName = "Romance";
//            db.Genres.AddOrUpdate(g => g.GenreName, g3);
//            db.SaveChanges();

//            Genre g4 = new Genre();
//            g4.GenreName = "Fantasy";
//            db.Genres.AddOrUpdate(g => g.GenreName, g4);
//            db.SaveChanges();

//            Genre g5 = new Genre();
//            g5.GenreName = "Animation";
//            db.Genres.AddOrUpdate(g => g.GenreName, g5);
//            db.SaveChanges();

//            Genre g6 = new Genre();
//            g6.GenreName = "Family";
//            db.Genres.AddOrUpdate(g => g.GenreName, g6);
//            db.SaveChanges();


//        }
//    }
//}