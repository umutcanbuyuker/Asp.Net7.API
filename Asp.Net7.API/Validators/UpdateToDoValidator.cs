using Asp.Net7.API.DTOs;
using FluentValidation;

namespace Asp.Net7.API.Validators
{
    public class UpdateToDoValidator : AbstractValidator<ToDoForUpdatedDto>
    {
        public UpdateToDoValidator()
        {
            RuleFor(toDo => toDo.Id).
                NotEmpty().
                GreaterThan(0);
            RuleFor(toDo => toDo.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen ToDo adını boş geçmeyiniz.")
                .MaximumLength(100)
                .MinimumLength(5)
                    .WithMessage("Lütfen ToDo adını 5 ile 100 karakter arasında giriniz.");
            RuleFor(toDo => toDo.Category)
               .NotEmpty()
               .NotNull()
                   .WithMessage("Lütfen ToDo kategorisini boş geçmeyiniz.")
               .MaximumLength(40)
               .MinimumLength(2)
                   .WithMessage("Lütfen ToDo kategorisini 2 ile 40 karakter arasında giriniz.");
        }
    }
}
