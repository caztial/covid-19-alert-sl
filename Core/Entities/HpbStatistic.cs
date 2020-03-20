using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class HpbStatistic
    {
        public Guid Id { get; set; }
        public int LocalNewCases { get; set; }
        public int LocalTotalCases { get; set; }
        public int LocalTotalNumberOfIndividualsInHospitals { get; set; }
        public int LocalDeaths { get; set; }
        public int LocalNewDeaths { get; set; }
        public int LocalRecoverd { get; set; }
        public int GlobalNewCases { get; set; }
        public int GlobalTotalCases { get; set; }
        public int GlobalDeaths { get; set; }
        public int GlobalNewDeaths { get; set; }
        public int GlobalRecovered { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
