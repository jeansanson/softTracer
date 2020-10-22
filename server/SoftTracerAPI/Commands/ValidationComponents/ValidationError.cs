

using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SofTracerAPI.Commands
{
    public class ValidationError
    {
        public ValidationError(List<string> errors)
        {
            IsInvalid = errors.Count > 0;
            Errors = errors;
            Error = string.Join(", ", errors);
        }

        public bool IsInvalid { get;  }

        public List<string> Errors { get; }

        public string Error { get; }
    }
}