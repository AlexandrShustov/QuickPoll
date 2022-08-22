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

  public Task<long> DeObfuscate(string id) => 
    Task.FromResult(_hasher.DecodeSingleLong(id));
}