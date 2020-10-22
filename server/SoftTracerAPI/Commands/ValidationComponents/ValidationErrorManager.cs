using Org.BouncyCastle.Bcpg;
using System.Collections.Generic;
using System.Linq;

namespace SofTracerAPI.Commands
{
    public class ValidationErrorManager
    {
        private List<string> _errors;

        ValidationErrorManager()
        {
            _errors = new List<string>();
        }

        void AddError(string error)
        {
            _errors.Add(error);
        }

        string GetErrors()
        {
            return string.Join(", ",_errors);
        }
    }
}