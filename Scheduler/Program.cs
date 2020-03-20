using Application.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Scheduler
{
    class Program
    {
      
        static async Task Main(string[] args)
        {
            
            var config = GetConfiguration();
            Console.WriteLine("CLI Running");

            HpbApiService HbpApiService = new HpbApiService(config);
            await HbpApiService.GetStatusReport();

        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"cli.appsettings.json", false, true);
            return builder.Build();
        }

    }
}
