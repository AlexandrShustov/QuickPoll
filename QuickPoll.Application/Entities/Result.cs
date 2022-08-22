namespace QuickPoll.Application.Entities;

public interface IResult
{ }

public interface IResult<T> : IResult
{
  public T Value { get; set; }
}

public class OkResult : IResult
{
}

public class OkResult<T> : OkResult, IResult<T>
{
  public T Value { get; set; }

  public OkResult(T value)
  {
    Value = value;
  }
}

public class ErrorResult : IResult
{
  public string Message { get; set; }

  public ErrorResult WithMessage(string message)
  {
    Message = message;
    return this;
  }
}

public class ErrorResult<T> : ErrorResult, IResult<T>
{
  public T Value { get; set; }

  public ErrorResult(T value)
  {
    Value = value;
  }

  public new ErrorResult<T> WithMessage(string message)
  {
    base.WithMessage(message);
    return this;
  }
}

public class NotFoundResult : ErrorResult
{ }

public class NotFoundResult<T> : ErrorResult<T>
{
  public NotFoundResult(T value) : base(value)
  {
    Message = "The queried entity wasn't found.";
  }
}