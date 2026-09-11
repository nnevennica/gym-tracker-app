using GymTracker.Application.Interfaces;
using GymTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Infrastructure.Persistence.Repositories;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly ApplicationDbContext _context;

    public WorkoutRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Workout?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Workouts
            .Include(w => w.ExerciseType)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Workout>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Workouts
            .Include(w => w.ExerciseType)
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.DateTime)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Workout workout, CancellationToken cancellationToken = default)
    {
        await _context.Workouts.AddAsync(workout, cancellationToken);
    }

    public void Update(Workout workout)
    {
        _context.Workouts.Update(workout);
    }

    public void Delete(Workout workout)
    {
        _context.Workouts.Remove(workout);
    }
}