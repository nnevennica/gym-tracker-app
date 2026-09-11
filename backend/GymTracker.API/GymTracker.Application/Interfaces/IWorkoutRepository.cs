using GymTracker.Domain.Entities;

namespace GymTracker.Application.Interfaces;

public interface IWorkoutRepository
{
    Task<Workout?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Workout>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(Workout workout, CancellationToken cancellationToken = default);
    void Update(Workout workout);
    void Delete(Workout workout);
}