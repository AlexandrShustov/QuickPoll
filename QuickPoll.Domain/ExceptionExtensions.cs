namespace QuickPoll.Domain;

public static class ExceptionExtensions
{
  public static Exception Unwrap(this Exception self)
  {
    var wrappedException = self;
    while (wrappedException.InnerException != null)
    {
      wrappedException = wrappedException.InnerException;
    }

    return wrappedException;
  }
}