using System;
using System.Data.Entity;

namespace Bash
{
	public class BashDbContext : DbContext
	{
		public BashDbContext()
			: base("MonkeyFist")
		{ 
			//Database.SetInitializer<BashDbContext>(new CreateDatabaseIfNotExists<BashDbContext>());
			Database.SetInitializer<BashDbContext>(new DropCreateDatabaseIfModelChanges<BashDbContext>());
			//Database.SetInitializer<BashDbContext>(new Mig());
		}

		public DbSet<User> Users { get; set; }
	}
}
