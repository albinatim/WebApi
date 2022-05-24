using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class UserContext: DbContext
    {
        public UserContext(DbContextOptions<UserContext> options):base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<User>().HasData(new User
        //    {
        //        Login = "AdminUser1",
        //        Password = "123456",
        //        Name = "John",
        //        Gender = 0,
        //        Birthday = null,
        //        Admin = true,
        //        CreatedOn = DateTime.Now,
        //        CreatedBy = "",
        //        ModifiedOn = DateTime.Now,
        //        ModifiedBy = "",
        //        RevokedOn = null,
        //        RevokedBy = ""
        //    });
        //}
    }
}
