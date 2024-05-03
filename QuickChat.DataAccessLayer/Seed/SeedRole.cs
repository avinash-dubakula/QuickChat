using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace QuickChat.DataAccessLayer.Seed
{
    public class SeedRole
    {
        public static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
              new IdentityRole { Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
              new IdentityRole { Id = "506a60bb-79f3-45a7-a022-53ec1e62061c", Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" },
              new IdentityRole { Id = "ff8f4a2b-67d6-434e-b6cb-8cfac026e4c5", Name = "SuperUser", ConcurrencyStamp = "3", NormalizedName = "HR" }
            );
        }

        public static void SeedUsers(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<IdentityUser>();
            var user = new IdentityUser
            {
                Id = "b74ddd14-6340-4840-95c2-db12554843e5",
                UserName = "Admin",
                Email = "admin@gmail.com",
                LockoutEnabled = false,
                PhoneNumber = "1234567890"
            };

            user.PasswordHash = hasher.HashPassword(user, "Admin*123");

            builder.Entity<IdentityUser>().HasData(user);
        }

        public static void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = "fab4fac1-c546-41de-aebc-a14da6895711", UserId = "b74ddd14-6340-4840-95c2-db12554843e5" }
            );
        }
    }
}
