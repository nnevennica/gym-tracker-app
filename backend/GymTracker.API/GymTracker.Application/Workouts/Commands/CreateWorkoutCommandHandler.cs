using GymTracker.Application.Interfaces;
using GymTracker.Domain.Common;
using GymTracker.Domain.Entities;
using MediatR;

namespace GymTracker.Application.Workouts.Commands;

public class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, Result<Guid>>
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWorkoutCommandHandler(IWorkoutRepository workoutRepository, IUnitOfWork unitOfWork)
    {
        _workoutRepository = workoutRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
    {
        var workout = new Workout(
            request.UserId,
            request.ExerciseTypeId,
            request.DateTime,
            request.DurationMinutes,
            request.CaloriesBurned,
            request.IntensityRating,
            request.FatigueRating,
            request.Notes
        );

        await _workoutRepository.AddAsync(workout, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(workout.Id);
    }
}