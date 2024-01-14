using System.Collections.Generic;

namespace Timelogger.Commands
{
    public class CommandResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public List<string> Errors { get; private set; }

        private CommandResult(bool isSuccess, string message, List<string> errors)
        {
            IsSuccess = isSuccess;
            Message = message;
            Errors = errors;
        }

        public static CommandResult Success(string message = "")
        {
            return new CommandResult(true, message, null);
        }

        public static CommandResult Fail(string errorMessage)
        {
            return new CommandResult(false, "", new List<string> { errorMessage });
        }
    }
}