using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using YomiOlatunji.Core.DbModel;
using System;
using YomiOlatunji.Core.Enums;

namespace YomiOlatunji.DataSource.Services
{
    public class SeedUserDataService
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                if (context == null)
                    return;

                string[] roles = new string[] { "Owner", "Administrator", "Manager", "Editor", "Reader", "Subscriber" };

                foreach (string role in roles)
                {
                    var roleStore = new RoleStore<IdentityRole>(context);

                    if (!context.Roles.Any(r => r.Name == role))
                    {
                        var s=Task.Run(async () => await roleStore.CreateAsync(new IdentityRole()
                        {
                            ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                            Id = Guid.NewGuid().ToString("D"),
                            Name = role,
                            NormalizedName = role.ToUpper()
                        }));
                        var d = s.Result;
                    }
                }


                var user = new ApplicationUser()
                {
                    FirstName = "Yomi",
                    LastName = "Olatunji",
                    Email = "mosesoluwayomi@gmail.com",
                    NormalizedEmail = "mosesoluwayomi@GMAIL.COM",
                    UserName = "Administrator",
                    NormalizedUserName = "Administrator",
                    PhoneNumber = "+2348067363390",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };


                if (!context.Users.Any(u => u.UserName == user.UserName))
                {
                    var password = new PasswordHasher<ApplicationUser>();
                    var hashed = password.HashPassword(user, "Password1$");
                    user.PasswordHash = hashed;

                    //var userStore = new UserStore<ApplicationUser>(context); 
                    var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                    var task = userManager?.CreateAsync(user).Result;
                    AssignRoles(serviceScope.ServiceProvider, user.Email, roles);
                }


                context.SaveChangesAsync();
            }
        }

        public static void AssignRoles(IServiceProvider services, string email, string[] roles)
        {
            var userManager = services.GetService<UserManager<ApplicationUser>>();
            if (userManager != null)
            {
                var user = userManager.FindByEmailAsync(email).Result;
               var tt= Task.Run(async()=>await userManager.AddToRolesAsync(user, roles));
               var ss = tt.Result;
            }

        }
    }
}
