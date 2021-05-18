using System;

namespace MotorcyclePartManagerWebApi.CustomExceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
        
    }
}
