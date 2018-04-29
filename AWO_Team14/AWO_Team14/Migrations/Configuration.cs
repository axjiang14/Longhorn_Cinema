namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AWO_Team14.DAL.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AWO_Team14.DAL.AppDbContext context)
        {
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data.

			//GenreData AddGenres = new GenreData();
			//AddGenres.SeedGenres(context);

			//MovieData AddMovies = new MovieData();
			//AddMovies.SeedMovies(context);

			//UserData AddUsers = new UserData();
			//AddUsers.SeedUsers(context);

			//seed genres
			GenreData AddGenres = new GenreData();
			AddGenres.SeedGenres(context);

			//seed movies
			MovieData AddMovies = new MovieData();
			AddMovies.SeedMovies(context);

            SeedIdentity si = new SeedIdentity();
            si.AddAdmin(context);
        }

    }
}
