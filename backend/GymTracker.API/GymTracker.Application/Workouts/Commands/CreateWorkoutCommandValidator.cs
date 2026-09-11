using FluentValidation;

namespace GymTracker.Application.Workouts.Commands;

public class CreateWorkoutCommandValidator : AbstractValidator<CreateWorkoutCommand>
{
    public CreateWorkoutCommandValidator()
    {
        RuleFor(x => x.ExerciseTypeId).NotEmpty();
        RuleFor(x => x.DurationMinutes).GreaterThan(0).WithMessage("Trajanje mora biti veće od 0 minuta.");
        RuleFor(x => x.CaloriesBurned).GreaterThanOrEqualTo(0);
        RuleFor(x => x.IntensityRating).InclusiveBetween(1, 10).WithMessage("Ocena težine mora biti između 1 i 10.");
        RuleFor(x => x.FatigueRating).InclusiveBetween(1, 10).WithMessage("Ocena umora mora biti između 1 i 10.");
    }
}