using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.ProjectIdentity
{
    public class AuthenticationResponseDTO
    {
        public bool IsAuthSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set;}
        public UserForRetrunDTO userForRetrunDTO { get; set; }
    }
   
}
