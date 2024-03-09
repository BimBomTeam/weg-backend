using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WEG.Domain.DataAccess
{
	public class UsersContext : IdentityDbContext
	{
		UsersContext(DbContextOptions options) : base(options) { }
		public DbSet<Entities.User> Users { get; set; }
	}
}

