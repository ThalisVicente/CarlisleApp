using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Classes
{
    public class DBRequestResult
    {
        public DBRequestResult(bool success)
        {
            Success = success;
        }

        public bool Success { get; set; }

        public string Message
        {
            get
            {
                return Success ? "Success!" : "Something went wrong, please contact the Developer team.";
            }
        }
    }
}
