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
}

public class ErrorResult<T> : ErrorResult, IResult<T>
{
  public T Value { get; set; }

  public ErrorResult(T value)
  {
    Value = value;
  }
}