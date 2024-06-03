using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS
{
    public class HotelRoomDTOS
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Room Name is Required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Occupancy  is Required")]

        public int Occupancy { get; set; }
        [Required(ErrorMessage = "Price must be between 1 and 2000$")]

        public double Price { get; set; }
        public string? Details { get; set; }
        public string? Area { get; set; }
        public bool IsBooked { get; set; }
        public double TotalAmount { get; set; }
        public int TotalNights { get; set; }

        public List<string> ImageUrls { get; set; }
        public virtual ICollection<RoomImageDTO>? RoomImages { get; set; }


    }
}
