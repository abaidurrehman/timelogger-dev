using System;
using System.Collections.Generic;
using FluentValidation.Results;

namespace Timelogger.Behaviors
{
    public class ValidationException : Exception
    {
        public ValidationException(List<ValidationFailure> failures) : base("Validation failed")
        {
            Failures = failures;
        }

        public List<ValidationFailure> Failures { get; }
    }
}