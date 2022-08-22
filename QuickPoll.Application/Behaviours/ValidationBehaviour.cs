using FluentValidation;
using MediatR;

namespace QuickPoll.Application.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
  private readonly IEnumerable<IValidator<TRequest>> _validators;

  public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
  {
    _validators = validators;
  }

  public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
  {
    if (!_validators.Any())
      return await next.Invoke();

    var context = new ValidationContext<TRequest>(request);

    var result = _validators
      .Select(x => x.Validate(context))
      .SelectMany(x => x.Errors)
      .ToList();

    if (result.Any())
      throw new ValidationException(result);

    return await next.Invoke();
  }
}