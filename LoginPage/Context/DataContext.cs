using System;
using System.Collections.Generic;
using LoginPage.Entities;
using Microsoft.EntityFrameworkCore;
using Task = LoginPage.Entities.Task;

namespace LoginPage.Context;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }
    

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { set; get; }
    public virtual DbSet<Status> Status { get; set; }
    public virtual DbSet<UserTask> UsersTasks { get; set; }
    public virtual DbSet<Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Only configure if no options were provided (which should not happen in production)
        if (!optionsBuilder.IsConfigured)
        {
            // Fallback connection string for development/testing only
            // In Docker environment, this should never be reached
            optionsBuilder.UseNpgsql("Host=db;Port=5432;Database=postgres;Username=postgres;Password=123456789;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("pgcrypto")
            .HasPostgresExtension("uuid-ossp");
        
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");
            entity.ToTable("roles");
        
            entity.Property(e => e.Id)
                .HasColumnName("id");
        
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            
            
            
            entity.HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );
        });
        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("status_pkey");
            entity.ToTable("status");

            entity.Property(e => e.Id)
                .HasColumnName("id");
            
            entity.Property(e => e.Code)
                .HasColumnName("code");


            entity.HasData(
                new Status { Id = 1, Code = "ToDo" },
                new Status { Id = 2, Code = "InProgress" },
                new Status { Id = 3, Code = "Completed" }
                );

        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_pkey");
            entity.ToTable("task");
            
            entity.Property(e => e.Id)
                .HasColumnName("id");
            
            entity.Property(e => e.Title)
                .HasColumnName("title");
            
            entity.Property(e => e.Description)
                .HasColumnName("description");
            
            entity.Property(e => e.StatusId).HasColumnName("statusid");
            
            entity.HasOne(d => d.Status)
                .WithMany()
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("createdat")
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updatedat");
                
            
            entity.Property(e => e.UntilAt)
                    .HasColumnName("untilat");
        });
        modelBuilder.Entity<UserTask>(entity =>
        {
            entity.HasKey(ut => new { ut.UserId, ut.TaskId });
            entity.HasOne(ut => ut.User)
                .WithMany(u => u.UserTasks)
                .HasForeignKey(ut => ut.UserId);

            entity.HasOne(ut => ut.Task)
                .WithMany(t => t.UserTasks)
                .HasForeignKey(ut => ut.TaskId);

            entity.ToTable("users_tasks");
        });
        
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("roleid");
            entity.HasOne(d => d.Role)
                .WithMany()
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}