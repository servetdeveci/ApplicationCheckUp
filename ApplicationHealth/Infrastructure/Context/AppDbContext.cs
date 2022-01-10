using ApplicationHealth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationHealth.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<AppDef> Applications { get; set; }
        public DbSet<AppNotification> AppNotifications { get; set; }
        public DbSet<AppContact> AppNotificationContacts { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppDef>()
                         .HasKey(k => k.AppDefId);
            modelBuilder.Entity<AppDef>()
               .Property(p => p.AppDefId)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<AppNotification>()
                         .HasKey(k => k.AppNotificationId);
            modelBuilder.Entity<AppNotification>()
               .Property(p => p.AppNotificationId)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<AppContact>()
                         .HasKey(k => k.AppNotificationContactId);
            modelBuilder.Entity<AppContact>()
              .Property(p => p.AppNotificationContactId)
              .ValueGeneratedOnAdd();

            modelBuilder.Entity<AppDef>().Property(b => b.Name).HasMaxLength(50);

            modelBuilder.Entity<AppDef>()
                 .HasMany(app => app.Notifications)
                 .WithOne(appNoti => appNoti.AppDef)
                 .HasForeignKey(appNoti => appNoti.AppDefId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppDef>()
                 .HasMany(app => app.NotificationContacts)
                 .WithOne(appNotiCont => appNotiCont.AppDef)
                 .HasForeignKey(contact => contact.AppDefId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppNotification>()
                .HasOne(noti => noti.Contact)
                .WithMany(cont => cont.Notifications)
                .HasForeignKey(s => s.AppNotificationContactId);

            modelBuilder.Entity<AppContact>()
                            .HasIndex(noti => new {noti.AppDefId, noti.Phone, noti.Email })
                            .IsUnique();

        }

    }
}
