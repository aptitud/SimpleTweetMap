using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Tweetinvi;
using Tweetinvi.Core.Enum;

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

			//var tweets = Search.SearchTweets("Aptitud");

			var searchParameter = Search.GenerateSearchTweetParameter("Aptitud_Sthlm");
			//searchParameter.SearchType = SearchResultType.Recent;
			searchParameter.MaximumNumberOfResults = 100;
			var tweets = Search.SearchTweets(searchParameter);
			
			foreach (var tweet in tweets)
			{
				ParseTweet(tweet);
			}

			// Access the filtered stream
			var filteredStream = Stream.CreateFilteredStream();
			filteredStream.AddTrack("Aptitud_Sthlm");
			filteredStream.MatchingTweetReceived += (sender, args) => { ParseTweet(args.Tweet); };
			filteredStream.StartStreamMatchingAllConditions();
		}

		private void ParseTweet(Tweetinvi.Core.Interfaces.ITweet tweet)
		{
			if (tweet.Coordinates == null)
				return;
			var dto = new { coordinates = tweet.Coordinates, text = tweet.Text, sender = tweet.Creator.Name };

			GlobalHost.ConnectionManager.GetHubContext<UserHub>().Clients.All.tweet(dto);
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