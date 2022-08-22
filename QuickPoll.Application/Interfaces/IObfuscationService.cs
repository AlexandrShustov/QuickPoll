namespace QuickPoll.Application.Interfaces;

public interface IObfuscationService
{
  Task<string> Obfuscate(long id);
  Task<long> DeObfuscate(string id);
}