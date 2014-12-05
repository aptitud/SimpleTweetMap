using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Tweetinvi;

namespace SimpleTweetMap.Web
{
	public class Broadcaster
	{
		private readonly static Lazy<Broadcaster> instance = new Lazy<Broadcaster>(() =>
				new Broadcaster(GlobalHost.ConnectionManager.GetHubContext<UserHub>().Clients));

		private Broadcaster(IHubConnectionContext<dynamic> clients)
		{
			Clients = clients;
			Task.Factory.StartNew(()=>Init());
		}

		internal void Init()
		{
			var accessToken = System.Configuration.ConfigurationManager.AppSettings["accessToken"];
			var accessTokenSecret = System.Configuration.ConfigurationManager.AppSettings["accessTokenSecret"];
			var consumerKey = System.Configuration.ConfigurationManager.AppSettings["consumerKey"];
			var consumerSecret  = System.Configuration.ConfigurationManager.AppSettings["consumerSecret"];

			// Setup your application credentials
			TwitterCredentials.ApplicationCredentials = TwitterCredentials.CreateCredentials(accessToken, accessTokenSecret, consumerKey, consumerSecret);

			// Access the filtered stream
			var filteredStream = Stream.CreateFilteredStream();
			filteredStream.AddTrack("Aptitud_Sthlm");
			filteredStream.MatchingTweetReceived += (sender, args) => { GlobalHost.ConnectionManager.GetHubContext<UserHub>().Clients.All.hello(args.Tweet.Text); };
			filteredStream.StartStreamMatchingAllConditions();
		}

		public static Broadcaster Instance
		{
			get
			{
				return instance.Value;
			}
		}

		private IHubConnectionContext<dynamic> Clients
		{
			get;
			set;
		}

	}
}