using FluentValidation;
using TaskManagement.Api.DTOs;

public class UpdateTaskValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Priority)
            .IsInEnum();

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}