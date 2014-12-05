using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SimpleTweetMap.Web
{
	public class UserHub : Hub
	{
		private readonly Broadcaster broadCaster;

		public UserHub()
			: this(Broadcaster.Instance)
		{
		}

		public UserHub(Broadcaster broadCaster)
		{
			this.broadCaster = broadCaster;
		}

		public override System.Threading.Tasks.Task OnConnected()
		{
			broadCaster.LoadTweets();
			return base.OnConnected();
		}

		public void Hello(string message)
		{
			Clients.All.hello(message);
		}
	}
}