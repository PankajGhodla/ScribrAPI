using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ScribrAPI.CentralHub
{
    public class SignalRHub : Hub
    {
        public async Task BroadcastMessage()
        {
            await Clients.All.SendAsync("Connect");
        }

        public async Task MovieAdded()
        {
            await Clients.All.SendAsync("UpdateMovieList");
        }


    }
}
