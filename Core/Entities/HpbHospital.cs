using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class HpbHospital
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameSinhala { get; set; }
        public string NameTamil { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public ICollection<HpbHospitalStatus> HospitalStatuses { get; set; }
    }
}
