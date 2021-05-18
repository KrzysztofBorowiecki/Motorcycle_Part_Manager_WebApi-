using FluentValidation;
using MotorcyclePartManagerWebApi.Entities;
using MotorcyclePartManagerWebApi.Models;
using System;
using System.Linq;

namespace MotorcyclePartManagerWebApi.Validators
{
    public class MotorcycleQueryValidator : AbstractValidator<MotorcycleQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };

        private string[] allowedSortByColumnNames =
            {nameof(Motorcycle.Brand), nameof(Motorcycle.EngineCapacity), nameof(Motorcycle.Model),};
        public MotorcycleQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1).WithMessage("Numer strony musi być większy od 0");
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", allowedPageSizes)}]");
                }
            });

            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sortowanie jest opcjonalne, lub musi byc w [{string.Join(",", allowedSortByColumnNames)}]");
        } 
    }
}
