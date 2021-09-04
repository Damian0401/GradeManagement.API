using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Presistence
{
    public class DataBaseContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Note> Notes { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .HasKey(x => x.Id);
            builder.Entity<Note>()
                .Property(x => x.Title)
                .HasMaxLength(255);

            var converner = new EnumToStringConverter<Gender>();

            builder.Entity<ApplicationUser>()
                .Property(x => x.Gender)
                .HasConversion(converner);
        }
    }
}
