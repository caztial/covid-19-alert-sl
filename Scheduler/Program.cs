using Application.Infastructure.Persistance;
using Application.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Scheduler
{
    class Program
    {

        public static DataContext dataContext;

        public static IConfiguration configuration;
        static async Task Main(string[] args)
        {
            
            configuration = GetConfiguration();
            dataContext = new DataContext(configuration);
            //dataContext.Database.EnsureDeleted();
            dataContext.Database.EnsureCreated();

            Console.WriteLine("CLI Running");

            while (true)
            {
                try
                {
                    HpbApiService HbpApiService = new HpbApiService(configuration, dataContext);
                    await HbpApiService.GetStatusReport();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Service Exception");
                    Console.WriteLine(e.ToString());
                }
                
                Thread.Sleep(1000 * 60 * 5);
            }
            

        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"cli.appsettings.json", false, true);
            return builder.Build();
        }

    }
}
