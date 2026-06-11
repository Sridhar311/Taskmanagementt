using FluentValidation;
using TaskManagement.Api.DTOs;

namespace TaskManagement.Api.Validators;

public class CreateTaskValidator
    : AbstractValidator<CreateTaskDto>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Priority must be Low, Medium or High");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid Status");
    }
}
        