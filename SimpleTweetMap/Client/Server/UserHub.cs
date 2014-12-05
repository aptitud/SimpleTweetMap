using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
	class UserHub : Hub
	{
		public void HelloWorld()
		{
			Clients.All.helloWorld("Hello world!");
		}
		public override Task OnConnected()
		{
			HelloWorld();
			return base.OnConnected();
		}
	}
}
