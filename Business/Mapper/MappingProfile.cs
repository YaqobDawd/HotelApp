using AutoMapper;
using DataAccess.Model;
using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<HotelRoomDTOS, HotelRoom>().ReverseMap();
            CreateMap<RoomImageDTO, RoomImage>().ReverseMap();
            CreateMap<HotelAmenityDTO,HotelAmenity>().ReverseMap();
            CreateMap<RoomOrderDetailDTO, RoomOrderDetail>().ReverseMap();

            CreateMap<RoomOrderDetail, RoomOrderDetailDTO>().ForMember(x => x.hotelRoomDTO,
                opt => opt.MapFrom(c => c.HotelRoom));
            CreateMap<RoomOrderDetailDTO, RoomOrderDetail>();
        }
    }
}
