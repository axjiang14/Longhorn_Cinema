using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AWO_Team14.Models;
using AWO_Team14.DAL;
using System.Data.Entity.Migrations;

namespace AWO_Team14.Migrations
{
    public class MovieData
    {
        public void SeedMovies(AppDbContext db)
        {
            Movie m1 = new Movie();
            m1.Title = "42nd Street";
            m1.Tagline = "OK. Say, Jones and Barry are doin' a show! - That's great. Jones and Barry are doin' a show.";
            m1.Overview = "A producer puts on what may be his last Broadway show, and at the last moment a chorus girl has to replace the star.";
            m1.ReleaseYear = new DateTime(1933, 2, 2);
            m1.MPAA_Rating = MPAA.Unrated;
            m1.Runtime = 89;
            m1.Actors = "Warner Baxter, Bebe Daniels, George Brent, Ruby Keeler, Guy Kibbee, Una Merkel";
            //m1.Genres = new List<Genre> () {db.Genres.Where GenreName == "Musical");

            m1.Genres = new List<Genre>();
            m1.Genres.Add(db.Genres.FirstOrDefault(x => x.GenreName == "Romance"));
            m1.Genres.Add(db.Genres.FirstOrDefault(x => x.GenreName == "Comedy"));
            m1.Genres.Add(db.Genres.FirstOrDefault(x => x.GenreName == "Musical"));

            db.Movies.AddOrUpdate(m => m.Title, m1);
            db.SaveChanges();

            Movie m2 = new Movie();
            m2.Title = "Snow White and the Seven Dwarfs";
            m2.Tagline = "The Happiest, Dopiest, Grumpiest, Sneeziest movie of the year.";
            m2.Overview = "A beautiful girl, Snow White, takes refuge in the forest in the house of seven dwarfs to hide from her stepmother, the wicked Queen. The Queen is jealous because she wants to be known as the fairest in the land, and Snow White's beauty surpasses her own.";
            m2.ReleaseYear = new DateTime(1937, 12, 20);
            m2.MPAA_Rating = MPAA.G;
            m2.Runtime = 83;
            m2.Actors = "James Stewart, Jean Arthur, Claude Rains, Edward Arnold, Guy Kibbee, Thomas Mitchell";
            m2.Genres = new List<Genre>();
            m2.Genres.Add(db.Genres.FirstOrDefault(x => x.GenreName == "Fantasy"));
            m2.Genres.Add(db.Genres.FirstOrDefault(x => x.GenreName == "Animation"));
            m2.Genres.Add(db.Genres.FirstOrDefault(x => x.GenreName == "Family"));
            db.Movies.AddOrUpdate(m => m.Title, m2);
            db.SaveChanges();

            

        }
    }
}