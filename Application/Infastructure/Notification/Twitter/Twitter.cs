using Core.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

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

        public void PostTweet(HpbStatistic hpbStatistic, string message, TwitterNotificationTypes type)
        {
            var userCredentials = Auth.CreateCredentials(Configuration["Twitter:ConsumerKey"], Configuration["Twitter:ConsumerSecret"],
                                                            Configuration["Twitter:UserToken"], Configuration["Twitter:UserSecret"]);
            AuthenticatedUser = User.GetAuthenticatedUser(userCredentials);  
            
            if(type == TwitterNotificationTypes.STATUS_UPDATE)
            {
                byte[] status_update_post = File.ReadAllBytes("status_update_" + hpbStatistic.Id + ".jpg");

                var publishedTweet = Auth.ExecuteOperationWithCredentials(userCredentials, () =>
                {
                    var publishOptions = new PublishTweetOptionalParameters();
                    if (status_update_post != null)
                    {
                        publishOptions.MediaBinaries.Add(status_update_post);
                    }

                    return Tweet.PublishTweet(message, publishOptions);
                });

            }
           
        }

        public void Publish(HpbStatistic hpbStatistic)
        {
            string tweet = "Total Cases - " + hpbStatistic.LocalTotalCases +"\n";
            tweet += "Active Cases - " + hpbStatistic.LocalActiveCases + "\n";
            tweet += "New Cases - " + hpbStatistic.LocalNewCases + "\n";
            tweet += "In Hospitals - " + hpbStatistic.LocalTotalNumberOfIndividualsInHospitals + "\n";
            tweet += "Total Recoverd - " + hpbStatistic.LocalRecoverd + "\n";
            tweet += "Total Deaths - " + hpbStatistic.LocalDeaths + "\n";
            tweet += "Updated on - " + hpbStatistic.LastUpdate + "\n";
            tweet += "More info visit https://www.hpb.health.gov.lk/" + "\n";
            tweet += "@HPBSriLanka #lka #COVID19SL #COVID19";

            PostTweet(hpbStatistic,tweet, TwitterNotificationTypes.STATUS_UPDATE);
        }
    }

    public enum TwitterNotificationTypes
    {
        STATUS_UPDATE
    }
}
