namespace QuickPoll.Application.Entities;

public class InvalidOptionResult : ErrorResult<string>
{
  public InvalidOptionResult(string value) : base(value)
  {
    Message = "The poll doesn't contain such option or it's doesn't exist.";
  }
}

public class NotFoundResult<T> : ErrorResult<T>
{
  public NotFoundResult(T value) : base(value)
  {
    Message = "The queried entity wasn't found.";
  }
}