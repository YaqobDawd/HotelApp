using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS
{
    public class HotelAmenityDTO
    {
        public int AmenityId { get; set; }
        [Required(ErrorMessage ="Amenity Name is required")]
        public string? AmenityName { get; set; }
        public string? AmenityDescription { get; set; }
        [Required(ErrorMessage = "Amenity Icon is required")]
        public string? AmenityIcon { get; set; }
        public string? AmenityTimming { get; set; }
    }
}
