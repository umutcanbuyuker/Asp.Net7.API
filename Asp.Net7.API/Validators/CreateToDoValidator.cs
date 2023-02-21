using Asp.Net7.API.DTOs;
using FluentValidation;

namespace Asp.Net7.API.Validators
{
    public class CreateToDoValidator : AbstractValidator<ToDoForCreatedDto>
    {
        public CreateToDoValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen ToDo adını boş geçmeyiniz.")
                .MaximumLength(100)
                .MinimumLength(5)
                    .WithMessage("Lütfen ToDo adını 5 ile 100 karakter arasında giriniz.");
            RuleFor(t => t.Category)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen ToDo adını boş geçmeyiniz.")
                .MaximumLength(40)
                .MinimumLength(5)
                    .WithMessage("Lütfen ToDo adını 5 ile 40 karakter arasında giriniz.");
        }
    }
}
