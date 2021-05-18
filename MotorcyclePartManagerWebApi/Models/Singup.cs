namespace MotorcyclePartManagerWebApi.Models
{
    public class Singup
    {
        public int Id{ get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int RoleId { get; set; } = 1002;
}
}
