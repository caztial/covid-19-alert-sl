using Core.Entities;
using Microsoft.Extensions.Configuration;
using System;
using Tweetinvi;
using Tweetinvi.Models;

namespace Application.Infastructure.Notification.Twitter
{
    public class Twitter: INotification
    {
        private IAuthenticatedUser AuthenticatedUser { get; set; }
        private IConfiguration Configuration { get; set; }

        public Twitter(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void PostTweet(string message)
        {
            var userCredentials = Auth.CreateCredentials(Configuration["Twitter:ConsumerKey"], Configuration["Twitter:ConsumerSecret"],
                                                            Configuration["Twitter:UserToken"], Configuration["Twitter:UserSecret"]);
            AuthenticatedUser = User.GetAuthenticatedUser(userCredentials);
            AuthenticatedUser.PublishTweet(message);
        }

        public void Publish(HpbStatistic hpbStatistic)
        {
            string tweet = "Total Cases " + hpbStatistic.LocalTotalCases +"\n";
            tweet += "New Cases " + hpbStatistic.LocalNewCases + "\n";
            tweet += "In Hospitals " + hpbStatistic.LocalTotalNumberOfIndividualsInHospitals + "\n";
            tweet += "Total Recoverd " + hpbStatistic.LocalRecoverd + "\n";
            tweet += "Total Deaths " + hpbStatistic.LocalNewDeaths + "\n";
            tweet += "Updated on " + hpbStatistic.LastUpdate + "\n";
            tweet += "More info visit https://www.hpb.health.gov.lk/" + "\n";
            tweet += "#lka #COVID19SL";

            PostTweet(tweet);
        }
    }
}
