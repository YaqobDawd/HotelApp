using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IRepo
{
    public interface IRoomImageRepo
    {
        public Task<int> CreateRooomImage(RoomImageDTO imageDTO);
        public Task<int> DeleteRoomImageByImageId(int ImageId);
        public Task<int> DeleterRoomImageByRoomId(int RoomId);
        public Task<int> DeleteRoomImageByImageUrl(string imageUrl);
        public Task<IEnumerable<RoomImageDTO>> GetRoomImages(int roomId);

    }
}
