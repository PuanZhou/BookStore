using Bulkybook.DataAcess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using BulkyBookweb.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkybook.DataAcess.DbIntializer
{
    public class DbInitialize : IDbInitializer
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;//建立RoleManager 透過DI注入
        private readonly ApplicationDbContext _db;//建立IUnitOfWork 透過DI注入
        public DbInitialize(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db=db;
        }
        public void Initialize()
        {
            //migrations if not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch(Exception ex)
            {

            }
            //create roles if they are not created

            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();//創造身分
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Indi)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Comp)).GetAwaiter().GetResult();

                //if roles are not created,then create admin
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName="Admin@gmail.com",
                    Email= "Admin@gmail.com",
                    Name="Admin",
                    PhoneNumber= "02-23113731",
                    StreetAddress= "中正區重慶南路一段122號",
                    State= "中正區",
                    PostalCode= "100",
                    City="台北市"

                },"Admin123*").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "Admin@gmail.com");

                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();

            }
            return;
        }
    }
}
