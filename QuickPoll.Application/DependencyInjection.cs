using System.Reflection;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using QuickPoll.Application.Behaviours;
using QuickPoll.Application.Models;

namespace QuickPoll.Application;

public static class DependencyInjection
{
  public static IServiceCollection AddApplication(this IServiceCollection self)
  {
    self.AddMediatR(Assembly.GetExecutingAssembly());
    self.AddTransient(typeof(IPipelineBehavior<,>),  typeof(ValidationBehaviour<,>));
    self.AddFluentValidation(x => x.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

    AddMapster(self);

    return self;
  }

  private static void AddMapster(IServiceCollection serviceCollection)
  {
    var config = new TypeAdapterConfig();

    config.Apply(new PollModel(), new RespondModel());

    serviceCollection.AddSingleton(config);
    serviceCollection.AddScoped<IMapper, ServiceMapper>();
  }
}