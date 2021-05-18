using MotorcyclePartManagerWebApi.Entities;
using MotorcyclePartManagerWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotorcyclePartManagerWebApi.Repositories
{
    public interface IPartRepository
    {
        Task AddPartAsync(Part part);
        Task DeletePartAsync(int id);
        Task<Part> GetPartAsync(int id);
        Task<PagedResult<Part>> GetPartsAsync(PartQuery query);
        Task UpdatePartAsync(Part part);
    }
}