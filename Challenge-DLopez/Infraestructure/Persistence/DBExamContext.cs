using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Persistence
{
	public class DBExamContext : DbContext
	{
		public virtual DbSet<Permission> Permissions { get; set; }
		public virtual DbSet<PermissionType> PermissionTypes { get; set; }

		public DBExamContext(DbContextOptions<DBExamContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(DBExamContext).Assembly);
			base.OnModelCreating(modelBuilder);
		}
	}
}
