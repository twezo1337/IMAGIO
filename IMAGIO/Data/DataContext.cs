using IMAGIO.Models;
using Microsoft.EntityFrameworkCore;
using IMAGIO.Entities;
using IMAGIO.Helpers;
using IMAGIO.Services;
using NuGet.ContentModel;

namespace IMAGIO.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Entities.Image> Images { get; set; }
        public DbSet<Entities.ImageTemp> ImageTemps { get; set; }
        public DbSet<Entities.FinalImage> FinalsImages { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("Users").HasKey(x => x.Id);

                builder.HasData(new User[]
                {
                    new User()
                    {
                        Id = 1,
                        Name = "WAWAWA",
                        Password = HashPasswordHelper.HashPassowrd("123456"),
                        Role = Role.UserPRO
                    },
                    new User()
                    {
                        Id = 2,
                        Name = "twezo",
                        Password = HashPasswordHelper.HashPassowrd("654321"),
                        Role = Role.User
                    }
                });

                builder.Property(x => x.Id).ValueGeneratedOnAdd();

                builder.Property(x => x.Password).IsRequired();
                builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Entities.Image>(builder =>
            {
                builder.ToTable("Images").HasKey(x => x.Id);

                builder.HasOne(r => r.User).WithMany(t => t.Images)
                    .HasForeignKey(r => r.UserId);
            });

            modelBuilder.Entity<Entities.ImageTemp>(builder =>
            {
                builder.ToTable("ImageTemps").HasKey(x => x.Id);

                builder.HasOne(r => r.Image).WithMany(t => t.ImageTemps)
                    .HasForeignKey(r => r.ImageId);
            });

            modelBuilder.Entity<Entities.FinalImage>(builder =>
            {
                builder.ToTable("Finalsimages").HasKey(x => x.Id);

                builder.HasOne(r => r.User).WithMany(t => t.FinalImages)
                    .HasForeignKey(r => r.UserId);
            });
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
