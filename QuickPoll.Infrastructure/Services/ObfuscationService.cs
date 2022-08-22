using HashidsNet;
using Microsoft.Extensions.Options;
using QuickPoll.Application.Interfaces;
using QuickPoll.Application.Options;

namespace QuickPoll.Infrastructure.Services;

public class ObfuscationService : IObfuscationService
{
  private readonly Hashids _hasher;

  public ObfuscationService(IOptions<HashIdOptions> options) => 
    _hasher = new Hashids(options.Value.Salt, options.Value.MinIdLength);

  public Task<string> Obfuscate(long id) => 
    Task.FromResult(_hasher.EncodeLong(id));

  public Task<(bool Success, long Result)> TryDeObfuscate(string id)
  {
    var success = _hasher.TryDecodeSingleLong(id, out var decodedId);
    return Task.FromResult((success, decodedId));
  }
}