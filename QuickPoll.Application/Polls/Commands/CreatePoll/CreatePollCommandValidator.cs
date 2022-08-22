using FluentValidation;

namespace QuickPoll.Application.Polls.Commands.CreatePoll;

public class CreatePollCommandValidator : AbstractValidator<CreatePollCommand>
{
  public CreatePollCommandValidator()
  {
    RuleFor(x => x.Question)
      .NotEmpty()
      .MaximumLength(200);

    RuleFor(x => x.Description)
      .MaximumLength(300)
      .When(x => x.Description is not null);

    RuleFor(x => x.Options)
      .NotNull()
      .Must(x => x.Count > 1)
      .WithMessage("The total amount of options should be more than 1.");

    RuleForEach(x => x.Options)
      .ChildRules(x =>
    {
      x.RuleFor(y => y.Text)
        .NotEmpty()
        .MaximumLength(300)
        .WithMessage("The option text length should be less than 300 symbols.");
    });
  }
}