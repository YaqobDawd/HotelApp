using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS
{
    public class ErrorDTO
    {
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
