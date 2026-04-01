using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Nimbleloop.AiBusinessAnaylst.WebApp.Data
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
	{
		public DbSet<Project> Projects { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<ApplicationUser>().ToCollection("users");
			builder.Entity<Project>().ToCollection("projects");
		}
	}
}
