using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrezUp.Core.Utils
{
   public class S3Result
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Url { get; set; } // במקרה של הצלחה, ה-URL של הקובץ
        public string ErrorMessage { get; set; } // במקרה של שגיאה
        public int StatusCode { get; set; }

    }
}
