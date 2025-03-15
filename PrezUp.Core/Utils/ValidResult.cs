using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrezUp.Core.Utils
{


    public class ValidResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }

        public ValidResult(bool isValid, string message)
        {
            IsValid = isValid;
            Message = message;
        }

        public static ValidResult Success() => new ValidResult(true, string.Empty);

        public static ValidResult Failure(string message) => new ValidResult(false, message);
    }


}
