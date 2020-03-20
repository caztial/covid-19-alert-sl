using Application.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class HpbApiService
    {
        private IConfiguration Configuration { get; set; }
        private static readonly HttpClient client = new HttpClient();
        public HpbApiService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task GetStatusReport()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "Covid19-Alter-SL");

            var resoponse = await client.GetAsync(Configuration["HBP:URL"]+ "get-current-statistical");
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
                Console.WriteLine("Endpoint error "+resoponse.StatusCode);
            }

            
        }
    }
}
