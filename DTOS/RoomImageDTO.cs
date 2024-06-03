using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS
{
    public class RoomImageDTO
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string? ImageUrl { get; set; }
        public virtual ICollection<RoomImageDTO>? RoomImages { get; set; }
    }
}
