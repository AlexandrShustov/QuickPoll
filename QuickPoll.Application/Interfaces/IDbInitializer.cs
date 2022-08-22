namespace QuickPoll.Application.Interfaces;

public interface IDbInitializer
{
  Task Seed(CancellationToken token);
}