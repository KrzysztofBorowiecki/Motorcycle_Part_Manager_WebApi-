using System.Security.Claims;

namespace MotorcyclePartManagerWebApi.Services.UserServices
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        int? GetUserId { get; }
    }
}
