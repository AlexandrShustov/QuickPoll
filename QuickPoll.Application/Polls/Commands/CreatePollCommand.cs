using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using QuickPoll.Application.Entities;
using QuickPoll.Application.Interfaces;
using QuickPoll.Application.Models;
using QuickPoll.Domain.Entities;

namespace QuickPoll.Application.Polls.Commands;

public class CreatePollCommandValidator : AbstractValidator<CreatePollCommand>
{
  public CreatePollCommandValidator()
  {
    RuleFor(x => x.Question).NotEmpty().MaximumLength(200);
    RuleFor(x => x.Description).MaximumLength(300).When(x => x.Description is not null);
    RuleFor(x => x.Options).NotNull()
      .Must(x => x.Count > 1)
      .WithMessage("The total amount of options should be more than 1.");
    
    RuleForEach(x => x.Options).ChildRules(x =>
    {
      x.RuleFor(y => y.Text)
        .NotEmpty()
        .MaximumLength(300)
        .WithMessage("The option text length should be less than 300 symbols.");
    });
  }
}

public class CreatePollCommand : PollModel, IRequest<IResult>
{
}

public class CreatePollCommandHandler : IRequestHandler<CreatePollCommand, IResult>
{
  private readonly IDbContext _dbContext;
  private readonly IObfuscationService _obfuscationService;
  private readonly IMapper _mapper;

  public CreatePollCommandHandler(IDbContext dbContext, IObfuscationService obfuscationService, IMapper mapper)
  {
    _dbContext = dbContext;
    _obfuscationService = obfuscationService;
    _mapper = mapper;
  }

  public async Task<IResult> Handle(CreatePollCommand request, CancellationToken cancellationToken)
  {
    var poll = _mapper.From(request).AdaptToType<Poll>();

    _dbContext.Polls.Add(poll);

    await _dbContext.SaveChanges(cancellationToken);
    return new OkResult<object>(new { CreatedPollId = await _obfuscationService.Obfuscate(poll.Id) });
  }
}