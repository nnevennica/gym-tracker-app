using GymTracker.Application.Interfaces;
using GymTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<ExerciseType> ExerciseTypes => Set<ExerciseType>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ExerciseType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });

        builder.Entity<ExerciseType>().HasData(
            new ExerciseType(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Kardio"),
            new ExerciseType(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Trening snage"),
            new ExerciseType(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Fleksibilnost")
        );

        builder.Entity<Workout>(entity =>
        {
            entity.HasKey(w => w.Id);

            entity.Property(w => w.DurationMinutes).IsRequired();
            entity.Property(w => w.CaloriesBurned).IsRequired();

            entity.Property(w => w.IntensityRating).IsRequired();
            entity.Property(w => w.FatigueRating).IsRequired();

            entity.Property(w => w.Notes).HasMaxLength(500);

            entity.HasOne(w => w.ExerciseType)
                  .WithMany()
                  .HasForeignKey(w => w.ExerciseTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}