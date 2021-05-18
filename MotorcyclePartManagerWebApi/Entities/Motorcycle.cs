using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MotorcyclePartManagerWebApi.Entities
{
    public class Motorcycle
    {
        [Key]
        public int? Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal EngineCapacity { get; set; }
        public int ProductionYear { get; set; }

        public int? CreatedById { get; set; }

        public virtual User CreatedBy { get; set; }
        //public decimal TotalBikeHoursOfMotorcycle { get; set; }
        //public decimal MyBikeHoursOfMotorcycle { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Part> Parts { get; set; }
    }
}
