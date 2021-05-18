using FluentValidation;
using MotorcyclePartManagerWebApi.Data;
using MotorcyclePartManagerWebApi.Models;
using System.Linq;

namespace MotorcyclePartManagerWebApi.Validators
{
    public class SingupEntityValidator : AbstractValidator<Singup>
    {
        public SingupEntityValidator(ProjectContext _context)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword)
                .Equal(e => e.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) => 
                {
                  var isEmailExist = _context.Users.Any(u => u.Email == value);
                    if (isEmailExist)
                    {
                        _context.AddFailure("Email", "Email jest zajęty");
                    }
                });
        }
    }
}
