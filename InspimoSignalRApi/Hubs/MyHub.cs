using InspimoSignalRApi.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

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
    }
}
