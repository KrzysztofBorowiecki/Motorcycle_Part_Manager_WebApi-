using MotorcyclePartManagerWebApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotorcyclePartManagerWebApi.Repositories
{
    public interface IMotorcycleRepository
    {
        Task AddMotorcycleAsync(Motorcycle motorcycle);
        Task DeleteMotorcycleAsync(int id);
        Task<Motorcycle> GetMotorcycleAsync(int id);
        Task<List<Motorcycle>> GetMotorcyclesAsync();
        Task UpdateMotorcycleAsync(Motorcycle motorcycle);
    }
}