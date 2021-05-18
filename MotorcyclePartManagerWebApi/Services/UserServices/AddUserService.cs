using AutoMapper;
using MotorcyclePartManagerWebApi.Data;
using MotorcyclePartManagerWebApi.Entities;
using MotorcyclePartManagerWebApi.Models;
using System.Threading.Tasks;


namespace MotorcyclePartManagerWebApi.Services.UserServices
{
    public class AddUserService : IAddUserService
    {
        private readonly ProjectContext _context;
        private readonly IMapper _mapper;
        public AddUserService(ProjectContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddUserAsync(Singup singup)
        {
            var user = _mapper.Map<User>(singup);

            user.Password = BCrypt.Net.BCrypt.HashPassword(singup.Password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
