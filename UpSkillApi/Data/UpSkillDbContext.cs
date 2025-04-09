using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UpSkillApi.Models;

namespace UpSkillApi.Data;

public partial class UpSkillDbContext : DbContext
{
    public UpSkillDbContext(DbContextOptions<UpSkillDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Advertisement> Advertisements { get; set; }

    public virtual DbSet<ApplicationStatus> ApplicationStatuses { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientPost> ClientPosts { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<PaidJob> PaidJobs { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Sponsor> Sponsors { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VolunteeringApplication> VolunteeringApplications { get; set; }

    public virtual DbSet<VolunteeringJob> VolunteeringJobs { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    public virtual DbSet<WorkerApplication> WorkerApplications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Advertisement>(entity =>
        {
            entity.HasIndex(e => e.SponsorId, "IX_Advertisements_SponsorId");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Sponsor).WithMany(p => p.Advertisements)
                .HasForeignKey(d => d.SponsorId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ApplicationStatus>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(255);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasIndex(e => e.NationalId, "IX_Clients_NationalId").IsUnique();

            entity.HasIndex(e => e.UserId, "IX_Clients_UserId").IsUnique();

            entity.HasOne(d => d.User).WithOne(p => p.Client).HasForeignKey<Client>(d => d.UserId);
        });

        modelBuilder.Entity<ClientPost>(entity =>
        {
            entity.HasIndex(e => e.ClientId, "IX_ClientPosts_ClientId");

            entity.HasIndex(e => e.UserId, "IX_ClientPosts_UserId");

            entity.Property(e => e.Details).HasMaxLength(2550);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Profession).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Client).WithMany(p => p.ClientPosts)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.ClientPosts).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_Organizations_UserId").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(500);

            entity.HasOne(d => d.User).WithOne(p => p.Organization).HasForeignKey<Organization>(d => d.UserId);
        });

        modelBuilder.Entity<PaidJob>(entity =>
        {
            entity.HasIndex(e => e.OrganizationId, "IX_PaidJobs_OrganizationId");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Organization).WithMany(p => p.PaidJobs)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasIndex(e => e.ClientId, "IX_Ratings_ClientId");

            entity.HasIndex(e => e.WorkerId, "IX_Ratings_WorkerId");

            entity.Property(e => e.Comment).HasMaxLength(500);

            entity.HasOne(d => d.Client).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Worker).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Sponsor>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();
        });

        modelBuilder.Entity<VolunteeringApplication>(entity =>
        {
            entity.HasIndex(e => e.ApplicationStatusId, "IX_VolunteeringApplications_ApplicationStatusId");

            entity.HasIndex(e => e.ClientId, "IX_VolunteeringApplications_ClientId");

            entity.HasIndex(e => e.VolunteeringJobId, "IX_VolunteeringApplications_VolunteeringJobId");

            entity.HasIndex(e => e.WorkerId, "IX_VolunteeringApplications_WorkerId");

            entity.HasOne(d => d.ApplicationStatus).WithMany(p => p.VolunteeringApplications)
                .HasForeignKey(d => d.ApplicationStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Client).WithMany(p => p.VolunteeringApplications).HasForeignKey(d => d.ClientId);

            entity.HasOne(d => d.VolunteeringJob).WithMany(p => p.VolunteeringApplications).HasForeignKey(d => d.VolunteeringJobId);

            entity.HasOne(d => d.Worker).WithMany(p => p.VolunteeringApplications).HasForeignKey(d => d.WorkerId);
        });

        modelBuilder.Entity<VolunteeringJob>(entity =>
        {
            entity.HasIndex(e => e.ApplicationStatusId, "IX_VolunteeringJobs_ApplicationStatusId");

            entity.HasIndex(e => e.OrganizationId, "IX_VolunteeringJobs_OrganizationId");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.ApplicationStatus).WithMany(p => p.VolunteeringJobs)
                .HasForeignKey(d => d.ApplicationStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Organization).WithMany(p => p.VolunteeringJobs).HasForeignKey(d => d.OrganizationId);
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasIndex(e => e.NationalId, "IX_Workers_NationalId").IsUnique();

            entity.HasIndex(e => e.UserId, "IX_Workers_UserId").IsUnique();

            entity.Property(e => e.HourlyRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Profession).HasMaxLength(50);

            entity.HasOne(d => d.User).WithOne(p => p.Worker).HasForeignKey<Worker>(d => d.UserId);
        });

        modelBuilder.Entity<WorkerApplication>(entity =>
        {
            entity.HasIndex(e => e.ApplicationStatusId, "IX_WorkerApplications_ApplicationStatusId");

            entity.HasIndex(e => e.ClientPostId, "IX_WorkerApplications_ClientPostId");

            entity.HasIndex(e => e.PaidJobId, "IX_WorkerApplications_PaidJobId");

            entity.HasIndex(e => e.WorkerId, "IX_WorkerApplications_WorkerId");

            entity.Property(e => e.ApplicationType).HasMaxLength(50);

            entity.HasOne(d => d.ApplicationStatus).WithMany(p => p.WorkerApplications)
                .HasForeignKey(d => d.ApplicationStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ClientPost).WithMany(p => p.WorkerApplications).HasForeignKey(d => d.ClientPostId);

            entity.HasOne(d => d.PaidJob).WithMany(p => p.WorkerApplications).HasForeignKey(d => d.PaidJobId);

            entity.HasOne(d => d.Worker).WithMany(p => p.WorkerApplications).HasForeignKey(d => d.WorkerId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
