using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcyclePartManagerWebApi.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }

        public virtual ICollection<Motorcycle> Motorcycles { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
