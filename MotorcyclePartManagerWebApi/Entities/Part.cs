using System;
using System.ComponentModel.DataAnnotations;

namespace MotorcyclePartManagerWebApi.Entities
{
    public class Part
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Producer { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public decimal BikeHours { get; set; }

        public int BelongsToMotorcycleId { get; set; }
        public virtual Motorcycle BelendsTo { get; set; }

        public int MotorcycleId { get; set; }
        public virtual Motorcycle Motorcycle { get; set; }
    }
}
