using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Library.Classes
{
    public class DBRequestResult
    {
        public DBRequestResult(bool success)
        {
            Success = success;
            Message = success ? "Success!" : "Something went wrong, please contact the Development team.";
        }

        public DBRequestResult(Exception exception)
        {
            Success = false;
            Message = exception.ToString();
        }

        public bool Success { get; set; }

        public string Message { get; set; }
 
    }
}
