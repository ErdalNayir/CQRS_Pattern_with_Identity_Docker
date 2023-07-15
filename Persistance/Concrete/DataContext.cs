using Domain.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Concrete
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=reactivitiesdb;database=reactivitiesdb;user=erdal;password=*2001*2001*", new MySqlServerVersion("8.0.32"));
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

            //Creating primary key
            builder.Entity<ActivityAttendee>(x=>x.HasKey(aa=>new {aa.AppUserId,aa.ActivityId}));

           //MANY TO MANY RELATIONSHIP

            builder.Entity<ActivityAttendee>()
                .HasOne(u=>u.AppUser).WithMany(a=>a.Activities).HasForeignKey(aa=>aa.AppUserId);

			builder.Entity<ActivityAttendee>()
				.HasOne(u => u.Activity).WithMany(a => a.Attendees).HasForeignKey(aa => aa.ActivityId);
		}
	}
}
