namespace QuickPoll.Application.Interfaces;

public interface IObfuscationService
{
  Task<string> Obfuscate(long id);
  Task<(bool Success, long Result)> TryDeObfuscate(string id);
}