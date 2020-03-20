using Application.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Scheduler
{
    class Program
    {
      
        static void Main(string[] args)
        {
            
            var config = GetConfiguration();

            HpbApiService HbpApiService = new HpbApiService(config);
            
            Console.WriteLine("CLI Running");
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"cli.appsettings.json", false, true);
            return builder.Build();
        }

    }
}
