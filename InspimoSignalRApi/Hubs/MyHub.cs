using InspimoSignalRApi.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
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
        public static List<string> Names { get; set; }=new List<string>();

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
    }
}
