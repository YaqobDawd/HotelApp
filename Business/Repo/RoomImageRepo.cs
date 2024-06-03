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
    public class RoomImageRepo : IRoomImageRepo
    {
        private readonly ApplicationDBContext db;
        private readonly IMapper mapper;

        public RoomImageRepo(ApplicationDBContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<int> CreateRooomImage(RoomImageDTO imageDTO)
        {
            
            var image=mapper.Map<RoomImageDTO,RoomImage>(imageDTO);
            await db.RoomImage.AddAsync(image);
            return await db.SaveChangesAsync();
        }

        public async Task<int> DeleteRoomImageByImageId(int ImageId)
        {
            var image = await db.RoomImage.FindAsync(ImageId);
            db.RoomImage.Remove(image);
            return await db.SaveChangesAsync();
        }

        public async Task<int> DeleteRoomImageByImageUrl(string ImageUrl)
        {
            var allImage=await db.RoomImage.FirstOrDefaultAsync(i=>i.ImageUrl.ToLower()==ImageUrl.ToLower());
            if (allImage==null)
            {
                return 0;
            }
            db.RoomImage.Remove(allImage);
            return await db.SaveChangesAsync();
        }

        public async Task<int> DeleterRoomImageByRoomId(int RoomId)
        {
            var imageList=await db.RoomImage.Where(i=>i.RoomId==RoomId).ToListAsync();
            db.RoomImage.RemoveRange(imageList);
            return await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoomImageDTO>> GetRoomImages(int roomId)
        {
            return mapper.Map<IEnumerable<RoomImage>,IEnumerable<RoomImageDTO>>(await db.RoomImage.Where(i=>i.RoomId==roomId).ToListAsync());
        }
    }
}
