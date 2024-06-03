using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class HotelAmenity
    {
        [Key]
        public int AmenityId { get; set; }
        [Required]

        public string? AmenityName { get; set; }
        public string? AmenityDescription { get; set; }
        [Required]

        public string? AmenityIcon { get; set; }
        public string? AmenityTimming { get; set; }
    }
}
