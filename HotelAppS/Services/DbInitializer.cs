using Common;
using DataAccess;
using DataAccess.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelAppS.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDBContext db;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public DbInitializer(ApplicationDBContext db,UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public void Initialize()
        {
            try
            {
                if (db.Database.GetPendingMigrations().Any())
                {
                    db.Database.Migrate();
                }
                if (db.Roles.Any(r => r.Name == SD.Role_Admin)) return;
                roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).Wait();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Cutstomer)).Wait();
                userManager.CreateAsync(new IdentityUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    
                },"Admin1!").Wait();
                IdentityUser? user = db.Users.FirstOrDefault(u => u.Email == "admin@gmail.com");
                userManager.AddToRoleAsync(user, SD.Role_Admin).Wait();
                
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
