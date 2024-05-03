using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using QuickChat.BusinessLogicLayer.Models.Entities;
using QuickChat.BusinessLogicLayer.Models.Entities.Identity;
using QuickChat.DataAccessLayer.Seed;

namespace QuickChat.DataAccessLayer.Context
{
    public class MyIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public MyIdentityDbContext(DbContextOptions<MyIdentityDbContext> options) : base(options)
        {

        }
        public DbSet<UserMessage> UserMessages { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        public DbSet<UserFriendRequest> UserFriendRequests { get; set; }
        public DbSet<UserGroupParticipant> UserGroupParticipants { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRole.SeedRoles(builder);
            //SeedRole.SeedUserRoles(builder);
            //SeedRole.SeedUsers(builder);
        }

    }
    public class MyIdentityDbContextFactory : IDesignTimeDbContextFactory<MyIdentityDbContext>
    {
        public MyIdentityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyIdentityDbContext>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=QuickChatIdentity;Integrated Security=True;");

            return new MyIdentityDbContext(optionsBuilder.Options);
        }
    }
}
