using Core.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Application.Infastructure.Notification.Facebook
{
    public class FacebookPage : INotification
    {
        private IConfiguration Configuration { get; set; }
        private HttpClient Client { get; set; }

        public FacebookPage(IConfiguration configuration)
        {
            Configuration = configuration;
            Client = new HttpClient(new HttpClientHandler());
        }
        public void Publish(HpbStatistic hpbStatistic)
        {
            string post = "Total Cases - " + hpbStatistic.LocalTotalCases + "\n";
            post += "Active Cases - " + hpbStatistic.LocalActiveCases + "\n";
            post += "New Cases - " + hpbStatistic.LocalNewCases + "\n";
            post += "In Hospitals - " + hpbStatistic.LocalTotalNumberOfIndividualsInHospitals + "\n";
            post += "Total Recoverd - " + hpbStatistic.LocalRecoverd + "\n";
            post += "Total Deaths - " + hpbStatistic.LocalNewDeaths + "\n";
            post += "Updated on - " + hpbStatistic.LastUpdate + "\n";
            post += "More info visit https://www.hpb.health.gov.lk/" + "\n";
            post += "#lka #COVID19SL #COVID19";

            var requestContent = new MultipartFormDataContent();
            HttpContent stringContent2 = new StringContent(post);
            
            HttpContent fileStreamContent = new StreamContent(new FileStream("status_update_" + hpbStatistic.Id + ".jpg", FileMode.Open,FileAccess.Read));
            fileStreamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

            requestContent.Add(stringContent2, "message");
            requestContent.Add(fileStreamContent, "file", "status_update_" + hpbStatistic.Id + ".jpg");
         
            var token = Configuration["Facebook:Token"];
            var response = Client.PostAsync($"https://graph.facebook.com/104399301213657/photos?access_token={token}", requestContent).Result;
            if (!response.IsSuccessStatusCode)
                Console.WriteLine("Error Posting Facebook " + response.StatusCode);
        }
    }
}
