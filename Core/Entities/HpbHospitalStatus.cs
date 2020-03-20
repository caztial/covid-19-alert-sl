using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    class HpbHospitalStatus
    {
        public int Id { get; set; }
        public int HospitalId { get; set; }
        public int CumulativeLocal { get; set; }
        public int CumulativeForeign { get; set; }
        public int TreatmentLocal { get; set; }
        public int TreatmentForeign { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public int CumulativeTotal { get; set; }
        public int TreatmentTotal { get; set; }
    }
}
