using Application.Infastructure.Notification;
using Application.Infastructure.Notification.Poster;
using Application.Infastructure.Notification.Twitter;
using Application.Infastructure.Persistance;

using Application.Models;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Application.Services
{
    public class HpbApiService
    {
        private IConfiguration Configuration { get; set; }
        private HttpClient Client { get; set; }
        private DataContext DataContext { get; set; }
        private List<INotification> Notifications { get; set; }
        public HpbApiService(IConfiguration configuration, DataContext dataContext)
        {
            Configuration = configuration;
            DataContext = dataContext;
            Client = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });

            Notifications = new List<INotification>
            {
                new ImageNotification(configuration),
                new Twitter(configuration)
            };
        }

        public async Task GetStatusReport()
        {
            var RetryGetRequest = Policy
              .Handle<HttpRequestException>()
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
                Console.WriteLine(DateTime.UtcNow.ToString()+" Refreshing Endpoint..... ");

                var resoponse = await Client.GetAsync(Configuration["HBP:URL"] + "get-current-statistical");
                if (resoponse.IsSuccessStatusCode)
                {
                    var contentStream = await resoponse.Content.ReadAsStringAsync();
                    var payload = JsonConvert.DeserializeObject<HpbStatisticResponse>(contentStream, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    await MapToDataContext(payload);
                    Console.WriteLine(DateTime.UtcNow.ToString() + " Refreshing Complete ");
                }
                else
                {
                    Console.WriteLine("Endpoint error " + resoponse.StatusCode);
                   
                }

            });
          
        }

        public async Task MapToDataContext(HpbStatisticResponse hpbStatisticResponse)
        {
            var response = hpbStatisticResponse.Data;
            // Check if the record existing in the database using the last updated time
            var flag = await DataContext.HpbStatistic
                .Where(u => u.LastUpdate.Equals(response.update_date_time))
                .AsNoTracking()
                .CountAsync();
            // if the record alredy existing skip this record;
            if (flag != 0)
                return;

            // Add the record to the Statistic table
            HpbStatistic hpbStatistic = new HpbStatistic 
            {
                LastUpdate = response.update_date_time,  
                LocalNewCases = response.local_new_cases,
                LocalDeaths = response.local_new_deaths,
                LocalNewDeaths = response.local_new_deaths,
                LocalRecoverd = response.local_recovered,
                LocalTotalCases = response.local_total_cases,
                LocalTotalNumberOfIndividualsInHospitals = response.local_total_number_of_individuals_in_hospitals,
                GlobalDeaths = response.global_deaths,
                GlobalNewCases = response.global_new_cases,
                GlobalNewDeaths = response.global_new_deaths,
                GlobalRecovered = response.global_recovered,
                GlobalTotalCases = response.global_total_cases                
            };

            DataContext.HpbStatistic.Add(hpbStatistic);
            hpbStatistic.HospitalStatuses = new List<HpbHospitalStatus>();
            // Add Hospital Status 
            foreach (var data in response.hospital_data)
            {
                // check if the hospital exisiting
                var hospital = await DataContext.HpbHospital
                    .Where(i => i.Id.Equals(data.hospital.id))
                    .FirstOrDefaultAsync();

                if (hospital == null)
                {
                    // if not create a new hospital
                    HpbHospital hpbHospital = new HpbHospital
                    {
                        Id = data.hospital.id,
                        Name = data.hospital.name,
                        NameSinhala = data.hospital.name_si,
                        NameTamil = data.hospital.name_ta,
                        CreatedAt = data.hospital.created_at,
                        UpdatedAt = data.hospital.updated_at,
                        DeletedAt = data.hospital.deleted_at,
                    };

                    DataContext.HpbHospital.Add(hpbHospital);
                    hospital = hpbHospital;
                }
                
                
                HpbHospitalStatus hpbHospitalStatus = new HpbHospitalStatus
                {
                    CreatedAt = data.created_at,
                    CumulativeForeign = data.cumulative_foreign,
                    CumulativeLocal = data.cumulative_local,
                    CumulativeTotal = data.cumulative_total,
                    DeletedAt = data.deleted_at,
                    UpdatedAt = data.updated_at,
                    TreatmentForeign = data.treatment_foreign,
                    TreatmentLocal = data.treatment_local,
                    TreatmentTotal = data.treatment_total,
                    HpbHospital = hospital
                };
               
                hpbStatistic.HospitalStatuses.Add(hpbHospitalStatus);
            }

            await NotificationTriggerCheck(hpbStatistic);
            await DataContext.SaveChangesAsync();
        }

        public async Task NotificationTriggerCheck(HpbStatistic hpbStatistic)
        {
            var lastRecord = await DataContext.HpbStatistic
                .OrderByDescending(d => d.LastUpdate)
                .FirstOrDefaultAsync();

            // if there is no record trigger the notification
            if (lastRecord == null)
            {
                await TriggerNotification(hpbStatistic);
         
            }
            else
            {
                // Local Cases increase or decrese
                if (lastRecord.LocalTotalCases != hpbStatistic.LocalTotalCases 
                    || lastRecord.LocalTotalNumberOfIndividualsInHospitals != hpbStatistic.LocalTotalNumberOfIndividualsInHospitals)
                    await TriggerNotification(hpbStatistic);
            }

        }

        public async Task TriggerNotification(HpbStatistic hpbStatistic)
        {
            Console.WriteLine("Total Cases " + hpbStatistic.LocalTotalCases);
            Console.WriteLine("New Cases " + hpbStatistic.LocalNewCases);
            Console.WriteLine("In Hospitals " + hpbStatistic.LocalTotalNumberOfIndividualsInHospitals);
            Console.WriteLine("Total Recoverd " + hpbStatistic.LocalRecoverd);
            Console.WriteLine("Total Deaths " + hpbStatistic.LocalNewDeaths);
            Console.WriteLine("More info visit https://www.hpb.health.gov.lk/");

            foreach (var notification in Notifications)
            {
                notification.Publish(hpbStatistic);
            }
        }
    }
}
