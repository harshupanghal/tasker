using Microsoft.EntityFrameworkCore;
using Tasker.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Tasker.Infrastructure.Persistence
    {
    public class AppDbContext : DbContext
        {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
            {
            }

        public DbSet<User> Users { get; set; }
        public DbSet<Domain.Entities.Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);

                entity.Property(u => u.userName).HasColumnName("userName");
                entity.Property(u => u.password).HasColumnName("password");
                entity.Property(u => u.CreatedAt).HasColumnName("createdAt");
            });

            modelBuilder.Entity<Domain.Entities.Task>(entity =>
            {
                entity.ToTable("Tasks");
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title).HasColumnName("title");
                entity.Property(t => t.Description).HasColumnName("description");
                entity.Property(t => t.IsCompleted).HasColumnName("isCompleted");
                entity.Property(t => t.UserId).HasColumnName("userId");
                entity.Property(t => t.CreatedAt).HasColumnName("createdAt");
                entity.Property(t => t.UpdatedAt).HasColumnName("updatedAt");
            });
            }
        }
    }
