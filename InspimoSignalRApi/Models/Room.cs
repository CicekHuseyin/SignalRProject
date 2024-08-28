using System.Collections.Generic;

namespace InspimoSignalRApi.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public string RoomName { get; set; }
        public List<User> Users { get; set; }
    }
}
