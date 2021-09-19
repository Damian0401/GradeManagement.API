using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Presistence
{
    public class DataBaseContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<Message> Messages { get; set; }

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
            builder.Entity<Message>()
                .Property(x => x.Title)
                .HasMaxLength(255);
            builder.Entity<Message>()
                .Property(x => x.IsRead)
                .HasDefaultValue(false);
            builder.Entity<Message>()
                .HasOne(x => x.UserFrom)
                .WithMany(x => x.SentMessages)
                .HasForeignKey(x => x.UserFromId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Message>()
                .HasOne(x => x.UserTo)
                .WithMany(x => x.ReceivedMessages)
                .HasForeignKey(x => x.UserToId)
                .OnDelete(DeleteBehavior.Restrict);

            var converner = new EnumToStringConverter<Gender>();

            builder.Entity<ApplicationUser>()
                .Property(x => x.Gender)
                .HasConversion(converner);
        }
    }
}
