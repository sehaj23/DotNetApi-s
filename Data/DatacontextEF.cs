using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace DotnetAPI.Data;

public class DataContextEF : DbContext
{
	private IConfiguration _config;
	
	public String connection;
	
	public DataContextEF(IConfiguration config)
	{
		_config = config;
		connection = _config.GetConnectionString("defaultConnection");

	}
	public virtual DbSet<User> Users { get; set; }
	public virtual DbSet<UsersSalary> UsersSalary { get; set; }
	public virtual DbSet<UsersJobInfo> UsersJobInfo { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("TutorialAppSchema");

		modelBuilder.Entity<User>().ToTable("Users").HasKey(c => c.UserId);

		modelBuilder.Entity<UsersSalary>().ToTable("UserSalary").HasKey(c => c.UserId);

		modelBuilder.Entity<UsersJobInfo>().ToTable("UserJobInfo").HasKey(c => c.UserId);
	}


protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
{
	if(!dbContextOptionsBuilder.IsConfigured)
	{
		dbContextOptionsBuilder.UseSqlServer(_config.GetConnectionString("defaultConnection"),dbContextOptionsBuilder => dbContextOptionsBuilder.EnableRetryOnFailure());
	}
}
	}
