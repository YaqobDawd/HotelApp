using AutoMapper;
using Business.IRepo;
using DataAccess;
using DataAccess.Model;
using DTOS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repo
{
    public class HotelAmenityRepo : IHotelAmenityRepo
    {
        private readonly ApplicationDBContext db;
        private readonly IMapper mapper;

        public HotelAmenityRepo(ApplicationDBContext db,IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<HotelAmenityDTO> CreateHotelAmenity(HotelAmenityDTO hotelAmenityDTO)
        {
            HotelAmenity Amenity = mapper.Map<HotelAmenityDTO, HotelAmenity>(hotelAmenityDTO); 
            var addAmenity= await db.HotelAmenity.AddAsync(Amenity);
            await db.SaveChangesAsync();
            return mapper.Map<HotelAmenity, HotelAmenityDTO>(addAmenity.Entity);
            
        }

        public async Task<int> DeleteHotelAmenity(int AmenityId)
        {
            var AmenityToDelete = await db.HotelAmenity.FindAsync(AmenityId);
            if (AmenityToDelete != null)
            {               
                db.HotelAmenity.Remove(AmenityToDelete);
                return await db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<HotelAmenityDTO>> GetAllHotelAmenity()
        {
            try
            {
                IEnumerable<HotelAmenityDTO> hotelAmenityDTOs =
                            mapper.Map<IEnumerable<HotelAmenity>, IEnumerable<HotelAmenityDTO>>
                            (db.HotelAmenity);

                return hotelAmenityDTOs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HotelAmenityDTO> GetHotelAmenity(int amenityId)
        {
            try
            {
                HotelAmenityDTO? AmenityDetail = mapper.Map<HotelAmenity, HotelAmenityDTO>(
                    await db.HotelAmenity.FirstOrDefaultAsync(r => r.AmenityId == amenityId));

                return AmenityDetail;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<bool> IsAmenityUnique(string AmenityName, int amenityId)
        {

            if (amenityId == 0)
            {
                //create
                HotelAmenity? hotelAmenity = await db.HotelAmenity.FirstOrDefaultAsync(r => r.AmenityName.ToLower() == AmenityName.ToLower());
                if (hotelAmenity == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //update
                HotelAmenity? hotelAmenity = await db.HotelAmenity.FirstOrDefaultAsync(r => r.AmenityName.ToLower() == AmenityName.ToLower() && r.AmenityId != amenityId);
                if (hotelAmenity == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<HotelAmenityDTO> UpdateHotelAmenity(HotelAmenityDTO hotelAmenityDTO, int amenityId)
        {
            try
            {
                if (amenityId == hotelAmenityDTO.AmenityId)
                {
                    HotelAmenity? Amenity2Update = await db.HotelAmenity.FindAsync(amenityId);
                    HotelAmenity Amenity = mapper.Map<HotelAmenityDTO, HotelAmenity>(hotelAmenityDTO, Amenity2Update);
                    
                    var updateAmenity = db.HotelAmenity.Update(Amenity);
                    await db.SaveChangesAsync();
                    return mapper.Map<HotelAmenity, HotelAmenityDTO>(updateAmenity.Entity);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
