

using System.Collections.Generic;
using System.Configuration;

namespace SofTracerAPI.Commands
{
    public class ValidationError
    {
        ValidationError(List<string> errors)
        {
            Valid = errors.Count == 0;
            Errors = errors;
        }

        public bool Valid { get;  }

        public List<string> Errors { get; }
    }
}