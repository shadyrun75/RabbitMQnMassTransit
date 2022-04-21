using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BasicResponse : IBasicResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
