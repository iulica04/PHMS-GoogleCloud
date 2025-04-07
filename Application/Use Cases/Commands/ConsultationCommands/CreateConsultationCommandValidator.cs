using Application.Use_Cases.Commands.ConsultationCommands;
using FluentValidation;

namespace Application.Use_Cases.CommandHandlers.ConsultationCommandHandler
{
    public class CreateConsultationCommandValidator : AbstractValidator<CreateConsultationCommand>
    {
        public CreateConsultationCommandValidator()
        {
            RuleFor(x => x.Date)
                 .GreaterThan(DateTime.Now.AddMinutes(45))
                 .WithMessage("Consultation date and time must be at least 15 minutes in the future.")
                 .Must(BeWithinWorkingHours)
                 .WithMessage("Consultation time must be between 08:00 and 18:00.")
                 .Must(NotBeOnWeekend)
                 .WithMessage("Consultation cannot be scheduled on a weekend.");

        }
        private bool BeWithinWorkingHours(DateTime dateTime)
        {
            var startHour = new TimeSpan(8, 0, 0);  // 08:00
            var endHour = new TimeSpan(18, 0, 0);   // 18:00
            var consultationTime = dateTime.TimeOfDay;

            return consultationTime >= startHour && consultationTime <= endHour;
        }
        private bool NotBeOnWeekend(DateTime dateTime)
        {
            return dateTime.DayOfWeek != DayOfWeek.Saturday && dateTime.DayOfWeek != DayOfWeek.Sunday;
        }
    }

}
