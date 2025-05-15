using System;
using Microsoft.EntityFrameworkCore;
using UpSkillApi.Models;

namespace UpSkillApi.Data
{
    public partial class UpSkillDbContext : DbContext
    {
        public UpSkillDbContext(DbContextOptions<UpSkillDbContext> options)
            : base(options) { }

        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<ApplicationStatus> ApplicationStatuses { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientPost> ClientPosts { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VolunteeringApplication> VolunteeringApplications { get; set; }
        public DbSet<VolunteeringJob> VolunteeringJobs { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<WorkerApplication> WorkerApplications { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<PostStatus> PostStatuses { get; set; }
        public DbSet<PendingRegistration> PendingRegistrations { get; set; }
        public DbSet<VolunteerPoints> VolunteerPoints { get; set; }
        public DbSet<UserAdvertisement> UserAdvertisements { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostStatus>(entity =>
            {
                entity.HasKey(e => e.PostStatusId);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.HasData(
                    new PostStatus { PostStatusId = 1, Name = "Posted", Description = "The post is publicly visible" },
                    new PostStatus { PostStatusId = 2, Name = "Done", Description = "The job is completed or closed" }
                );
            });

            modelBuilder.Entity<Profession>().HasData(
                new Profession { ProfessionId = 1, Name = "نجار" },
                new Profession { ProfessionId = 2, Name = "سباك" },
                new Profession { ProfessionId = 3, Name = "كهربائي" },
                new Profession { ProfessionId = 4, Name = "حداد" },
                new Profession { ProfessionId = 5, Name = "نقاش" },
                new Profession { ProfessionId = 6, Name = "عامل بناء" },
                new Profession { ProfessionId = 7, Name = "فني رخام" },
                new Profession { ProfessionId = 8, Name = "فني سيراميك" },
                new Profession { ProfessionId = 9, Name = "خياطة" },
                new Profession { ProfessionId = 10, Name = "سجاد يدوي" },
                new Profession { ProfessionId = 11, Name = "حفر علي الخشب" },
                new Profession { ProfessionId = 12, Name = "كروشيه و تريكوه" },
                new Profession { ProfessionId = 13, Name = "تطريز يدوي" },
                new Profession { ProfessionId = 14, Name = "اكسسوارات يدوية" },
                new Profession { ProfessionId = 15, Name = "صناعة شموع" },
                new Profession { ProfessionId = 16, Name = "صناعة فخار" },
                new Profession { ProfessionId = 17, Name = "الرسم" },
                new Profession { ProfessionId = 18, Name = "الرسم علي الزجاج" },
                new Profession { ProfessionId = 19, Name = "أخرى" }
            );

            modelBuilder.Entity<ApplicationStatus>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.HasData(
                    new ApplicationStatus { ApplicationStatusId = 1, Status = 1, Description = "Awaiting review", CreatedDate = DateTime.UtcNow },
                    new ApplicationStatus { ApplicationStatusId = 2, Status = 2, Description = "Application accepted", CreatedDate = DateTime.UtcNow },
                    new ApplicationStatus { ApplicationStatusId = 3, Status = 3, Description = "Application denied", CreatedDate = DateTime.UtcNow }
                );
            });

            modelBuilder.Entity<Advertisement>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasOne(d => d.Sponsor)
                      .WithMany(p => p.Advertisements)
                      .HasForeignKey(d => d.SponsorId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasIndex(e => e.NationalId).IsUnique();
                entity.HasIndex(e => e.UserId).IsUnique();
                entity.HasOne(d => d.User)
                      .WithOne(p => p.Client)
                      .HasForeignKey<Client>(d => d.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ClientPost>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(100);
                entity.Property(e => e.Location).HasMaxLength(200);
                entity.Property(e => e.Details).HasMaxLength(2550);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");

                entity.HasOne(d => d.Client)
                      .WithMany(p => p.ClientPosts)
                      .HasForeignKey(d => d.ClientId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(d => d.PostStatus)
                      .WithMany(s => s.ClientPosts)
                      .HasForeignKey(d => d.PostStatusId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(d => d.Profession)
                      .WithMany()
                      .HasForeignKey(d => d.ProfessionId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasIndex(e => e.UserId).IsUnique();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasOne(d => d.User)
                      .WithOne(p => p.Organization)
                      .HasForeignKey<Organization>(d => d.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
            

            modelBuilder.Entity<VolunteeringJob>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Location).HasMaxLength(200);
                entity.HasOne(d => d.Organization).WithMany(p => p.VolunteeringJobs).HasForeignKey(d => d.OrganizationId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(d => d.PostStatus).WithMany(s => s.VolunteeringJobs).HasForeignKey(d => d.PostStatusId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(u => u.PasswordResetCode).HasMaxLength(10);
                entity.Property(u => u.ResetCodeExpiry);
            });
            modelBuilder.Entity<User>()
                .Property(u => u.Points)
                .IsRequired()
                .HasDefaultValue(0);

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.HasIndex(e => e.NationalId).IsUnique();
                entity.HasIndex(e => e.UserId).IsUnique();
                entity.Property(e => e.HourlyRate).HasColumnType("decimal(18,2)");
                entity.HasOne(d => d.User).WithOne(p => p.Worker).HasForeignKey<Worker>(d => d.UserId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(d => d.Profession).WithMany(p => p.Workers).HasForeignKey(d => d.ProfessionId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<WorkerApplication>(entity =>
            {
                entity.HasOne(d => d.ApplicationStatus).WithMany(p => p.WorkerApplications).HasForeignKey(d => d.ApplicationStatusId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(d => d.ClientPost).WithMany(p => p.WorkerApplications).HasForeignKey(d => d.ClientPostId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(d => d.Worker).WithMany(p => p.WorkerApplications).HasForeignKey(d => d.WorkerId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.Property(e => e.Comment).HasMaxLength(500);
                entity.HasOne(d => d.Client).WithMany(p => p.Ratings).HasForeignKey(d => d.ClientId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(d => d.Worker).WithMany(p => p.Ratings).HasForeignKey(d => d.WorkerId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<VolunteeringApplication>(entity =>
            {
                entity.HasOne(d => d.ApplicationStatus).WithMany(p => p.VolunteeringApplications).HasForeignKey(d => d.ApplicationStatusId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(d => d.Client).WithMany(p => p.VolunteeringApplications).HasForeignKey(d => d.ClientId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(d => d.VolunteeringJob).WithMany(p => p.VolunteeringApplications).HasForeignKey(d => d.VolunteeringJobId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(d => d.Worker).WithMany(p => p.VolunteeringApplications).HasForeignKey(d => d.WorkerId).OnDelete(DeleteBehavior.NoAction);
            });
            
            modelBuilder.Entity<VolunteerPoints>(entity =>
            {
                entity.HasKey(e => e.VolunteerPointsId);

                entity.Property(e => e.Points).IsRequired();
                entity.Property(e => e.AwardedDate).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.VolunteerPoints)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(d => d.VolunteeringJob)
                    .WithMany(p => p.VolunteerPoints)
                    .HasForeignKey(d => d.VolunteeringJobId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
            
            modelBuilder.Entity<UserAdvertisement>(entity =>
            {
                entity.HasKey(e => e.UserAdvertisementId);

                entity.Property(e => e.RedeemedAt).IsRequired();
                entity.Property(e => e.RedeemCode).IsRequired().HasMaxLength(20);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserAdvertisements)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Advertisement)
                    .WithMany(a => a.UserAdvertisements)
                    .HasForeignKey(e => e.AdvertisementId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
            
            modelBuilder.Entity<Chat>(entity =>
            {
                entity.HasKey(e => e.ChatId);

                entity.HasOne(e => e.User1)
                    .WithMany(u => u.ChatsAsUser1)
                    .HasForeignKey(e => e.User1Id)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.User2)
                    .WithMany(u => u.ChatsAsUser2)
                    .HasForeignKey(e => e.User2Id)
                    .OnDelete(DeleteBehavior.NoAction);
            });
            

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}