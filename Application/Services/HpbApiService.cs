using Application.Infastructure.Persistance;
using Application.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Application.Services
{
    public class HpbApiService
    {
        private IConfiguration Configuration { get; set; }
        private HttpClient Client;
        private DataContext DataContext; 
        public HpbApiService(IConfiguration configuration, DataContext dataContext)
        {
            Configuration = configuration;
            DataContext = dataContext;
            Client = new HttpClient();
            

        }

        public async Task GetStatusReport()
        {
            var RetryGetRequest = Policy
              .Handle<Exception>()
              .WaitAndRetry(new[]
              {
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(15),
                TimeSpan.FromSeconds(30)
              });

            await RetryGetRequest.Execute(async () =>
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Add("User-Agent", "Covid19-Alert-SL");

                var resoponse = await Client.GetAsync(Configuration["HBP:URL"] + "get-current-statistical");
                if (resoponse.IsSuccessStatusCode)
                {
                    var contentStream = await resoponse.Content.ReadAsStringAsync();
                    var payload = JsonConvert.DeserializeObject<HpbStatisticResponse>(contentStream, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    Console.Write(payload.ToString());
                }
                else
                {
                    Console.WriteLine("Endpoint error " + resoponse.StatusCode);
                }

            });
            
            
        }
    }
}
