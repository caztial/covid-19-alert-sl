using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class HpbHospitalStatus
    {
        public Guid Id { get; set; }
        public int CumulativeLocal { get; set; }
        public int CumulativeForeign { get; set; }
        public int TreatmentLocal { get; set; }
        public int TreatmentForeign { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int CumulativeTotal { get; set; }
        public int TreatmentTotal { get; set; }
        public Guid HpbStatisticId { get; set; }
        [ForeignKey("HpbStatisticId")]
        public HpbStatistic HpbStatistic { get; set; }
        public int HpbHospitalId { get; set; }
        [ForeignKey("HpbHospitalId")]
        public HpbHospital HpbHospital { get; set; }
    }
}
