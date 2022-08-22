namespace QuickPoll.Domain.Exceptions;

public enum ErrorCode
{
  InvalidAnswer = 1
}

public class LogicException : Exception
{
  public ErrorCode Code { get; set; }
}