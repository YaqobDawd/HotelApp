using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.ProjectIdentity
{
    public class RegistrationResponseDTO
    {
        public bool IsRegistrationSuccess { get; set; }
        public IEnumerable<string>? Erorrs { get; set; }
    }
}
