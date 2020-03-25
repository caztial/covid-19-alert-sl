using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.Models
{
    public class HpbStatisticResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("data")]
        public HpbStatisticData Data { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class HpbStatisticData
    {
        public DateTime update_date_time { get; set; }
        public int local_new_cases { get; set; }
        public int local_total_cases { get; set; }
        public int local_total_number_of_individuals_in_hospitals { get; set; }
        public int local_deaths { get; set; }
        public int local_new_deaths { get; set; }
        public int local_recovered { get; set; }
        public int local_active_cases { get; set; }
        public int global_new_cases { get; set; }
        public int global_total_cases { get; set; }
        public int global_deaths { get; set; }
        public int global_new_deaths { get; set; }
        public int global_recovered { get; set; }
        public List<HpbHospitalStatusData> hospital_data { get; set; }
    }

    public class HpbHospitalStatusData
    {
        public int id { get; set; }
        public int hospital_id { get; set; }
        public int cumulative_local { get; set; }
        public int cumulative_foreign { get; set; }
        public int treatment_local { get; set; }
        public int treatment_foreign { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime deleted_at { get; set; }
        public int cumulative_total { get; set; }
        public int treatment_total { get; set; }
        public HpbHospitalData hospital { get; set; }
    }

    public class HpbHospitalData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string name_si { get; set; }
        public string name_ta { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime deleted_at { get; set; }
    }
}
