using IdentityModel;
using Mango.Services.Identity.DbContexts;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Mango.Services.Identity.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_roleManager.FindByNameAsync(StaticDetails.Admin).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Admin)).GetAwaiter().GetResult();

                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Customer)).GetAwaiter().GetResult();
            }
            else
            {
                return;
            }

            ApplicationUser adminUser = new()
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "9133204948",
                FirstName = "Medha",
                LastName = "Manchi"
            };

            _userManager.CreateAsync(adminUser, "Admin@123").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(adminUser, StaticDetails.Admin).GetAwaiter().GetResult();

            _userManager.AddClaimsAsync(adminUser, new Claim[] {
                new Claim(JwtClaimTypes.Name, adminUser.FirstName+ " "+ adminUser.LastName),
                new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
                new Claim(JwtClaimTypes.Role, StaticDetails.Admin),
            }).GetAwaiter().GetResult();

            ApplicationUser customerUser = new()
            {
                UserName = "customer@gmail.com",
                Email = "customer@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "9885110249",
                FirstName = "Venkata",
                LastName = "Rallabandi"
            };

            _userManager.CreateAsync(customerUser, "Customer@123").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(customerUser, StaticDetails.Customer).GetAwaiter().GetResult();

            _userManager.AddClaimsAsync(customerUser, new Claim[] {
                new Claim(JwtClaimTypes.Name, customerUser.FirstName+ " "+ customerUser.LastName),
                new Claim(JwtClaimTypes.GivenName, customerUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, customerUser.LastName),
                new Claim(JwtClaimTypes.Role, StaticDetails.Customer),
            }).GetAwaiter().GetResult();


        }
    }
}
