using MotorcyclePartManagerWebApi.Models;
using System.Threading.Tasks;

namespace MotorcyclePartManagerWebApi.Services.UserServices
{
    public interface IAddUserService
    {
        Task AddUserAsync(Singup singup);
    }
}