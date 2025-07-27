using Application.Common.Interfaces;
using Application.Features.Categories.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Validators
{
    public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator(IAppDbContext context)
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(c => c)
                .MustAsync(async (c, ct) => !await context.Categories.AnyAsync(x => x.Name == c.Name && x.ParentId == c.ParentId, ct))
                .WithMessage("A category with the same name and parent already exists.");
        }
    }
}


