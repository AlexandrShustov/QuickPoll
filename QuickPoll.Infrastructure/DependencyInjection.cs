using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickPoll.Application.Interfaces;
using QuickPoll.Application.Options;
using QuickPoll.Infrastructure.Services;
using QuickPoll.Infrastructure.Storage;

namespace QuickPoll.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection self, IConfiguration config)
  {
    self.Configure<HashIdOptions>(config.GetSection(HashIdOptions.Section));

    self.AddDbContext<ApplicationDbContext>(x => 
      x.UseInMemoryDatabase("quickPollsDb"));

    self.AddScoped<IDbContext>(x => x.GetRequiredService<ApplicationDbContext>());
    self.AddScoped<IDbInitializer, DbInitializer>();
    self.AddTransient<IObfuscationService, ObfuscationService>();

    return self;
  }
}