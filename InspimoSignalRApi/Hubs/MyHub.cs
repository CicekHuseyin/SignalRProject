using InspimoSignalRApi.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InspimoSignalRApi.Hubs
{
    public class MyHub : Hub
    {
        private readonly Context _contex;

        public MyHub(Context contex)
        {
            _contex = contex;
        }
        public static List<string> Names { get; set; } = new List<string>();

        public static int ClientCount { get; set; } = 0;
        public static int RoomCount { get; set; } = 5;
        public async Task SendName(string name)
        {
            Names.Add(name);
            await Clients.All.SendAsync("ReciveName", name);
        }
        public async Task GetNames()
        {
            await Clients.All.SendAsync("ReciveNames", Names);
        }
        public override async Task OnConnectedAsync()
        {
            ClientCount++;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            ClientCount--;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendNameByGroup(string name, string roomName)
        {
            var room = _contex.Rooms.Where(x => x.RoomName == roomName).FirstOrDefault();
            if (room != null)
            {
                room.Users.Add(new User { UserName = name });
            }
            else
            {
                var newRoom = new Room { RoomName = roomName, };
                newRoom.Users.Add(new User { UserName = name });
                _contex.Rooms.Add(newRoom);
            }
            await _contex.SaveChangesAsync();
            await Clients.Group(roomName).SendAsync("RecieveMessageByGroup",name,room.RoomID);
        }
        public async Task GetNamesByGroup()
        {
            var rooms = _contex.Rooms.Include(x => x.Users).Select(y => new
            {
                roomID=y.RoomID,
                users=y.Users.ToList()
            });
            await Clients.All.SendAsync("ReceiveNamesByGroup",rooms);
        }
        //Gruba Ekleme
        public async Task AddToGroup(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }
        //Grubdan Silme
        public async Task RemoveToGroup(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

    }
}
