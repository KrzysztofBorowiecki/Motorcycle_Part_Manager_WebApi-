using MotorcyclePartManagerWebApi.Models;

namespace MotorcyclePartManagerWebApi.Services.UserServices
{
    public interface IJwtTokenGeneratorService
    {
        string Generate(Login model);
    }
}