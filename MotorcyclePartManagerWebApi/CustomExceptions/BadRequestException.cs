using System;

namespace MotorcyclePartManagerWebApi.CustomExceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }  
    }
}
