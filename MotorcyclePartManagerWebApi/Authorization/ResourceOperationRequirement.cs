using Microsoft.AspNetCore.Authorization;

namespace MotorcyclePartManagerWebApi.Authorization
{
    public enum ResourceOperation
    {
        Add,
        Read,
        Update,
        Delete
    }
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperationRequirement(ResourceOperation resourceOperation)
        {
            ResourceOperation = ResourceOperation;
        }
        public ResourceOperation ResourceOperation { get; }
    }
}
