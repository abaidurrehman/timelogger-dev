using System;
using FluentValidation;

namespace Timelogger.Commands
{
    public class AddTimeRegistrationCommandValidator : AbstractValidator<AddTimeRegistrationCommand>
    {
        public AddTimeRegistrationCommandValidator()
        {
            RuleFor(v => v.TimeRegistration.ProjectId)
                .GreaterThan(0)
                .WithMessage("Project is required.");

            RuleFor(v => v.TimeRegistration.FreelancerId)
                .GreaterThan(0)
                .WithMessage("Freelancer is required.");

            RuleFor(v => v.TimeRegistration.TaskDescription)
                .NotEmpty()
                .WithMessage("Task description is required.");

            RuleFor(v => v.TimeRegistration.StartTime)
                .LessThan(v => v.TimeRegistration.EndTime)
                .WithMessage("End time should be greater than start time.");

            RuleFor(v => (v.TimeRegistration.EndTime - v.TimeRegistration.StartTime).TotalMinutes)
                .GreaterThanOrEqualTo(30)
                .WithMessage("Duration between Start Time and End Time should be at least 30 minutes.");

            RuleFor(v => v.TimeRegistration.StartTime)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Start time cannot be in the future.");

            RuleFor(v => v.TimeRegistration.EndTime)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("End time cannot be in the future.");
        }
    }
}