using DataAccess.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public partial class ApplicationDBContext:IdentityDbContext<IdentityUser>
    {
        public ApplicationDBContext(DbContextOptions <ApplicationDBContext> options):base (options)
        {
            
        }

        public DbSet<HotelRoom> HotelRooms { get; set; }
        public DbSet<RoomImage> RoomImage { get; set; }
        public DbSet<HotelAmenity> HotelAmenity { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<RoomOrderDetail> RoomOrderDetail { get; set;}
    }
}
