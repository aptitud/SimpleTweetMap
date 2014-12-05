using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SimpleTweetMap.Web
{
	public class UserHub : Hub
	{
		public void Hello()
		{
			Clients.All.hello("Hello World!");
		}

		public override System.Threading.Tasks.Task OnConnected()
		{
			Hello();
			return base.OnConnected();
		}
	}

}