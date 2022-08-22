using System.Net;
using System.Text.Json;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuickPoll.Application;
using QuickPoll.Application.Entities;
using QuickPoll.Application.Interfaces;
using QuickPoll.Application.Models;
using QuickPoll.Application.Polls.Commands.CreatePoll;
using QuickPoll.Application.Polls.Commands.RespondToPoll;
using QuickPoll.Application.Polls.Queries;
using QuickPoll.Application.Polls.Queries.GetPoll;
using QuickPoll.Application.Polls.Queries.GetResponds;
using QuickPoll.Domain;
using QuickPoll.Infrastructure;
using NotFoundResult = QuickPoll.Application.Entities.NotFoundResult;
using OkResult = QuickPoll.Application.Entities.OkResult;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  using var scope = app.Services.CreateScope();
  var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
  initializer.Seed(CancellationToken.None);

  app.UseWebAssemblyDebugging();
}

app.UseExceptionHandler(x => x.Run(async httpContext =>
{
  var handler = httpContext.Features.Get<IExceptionHandlerFeature>();
  if (handler is null)
    return;

  var response = handler.Error.Unwrap() switch
  {
    ValidationException ex => ValidationError(ex),
    { } ex => new { Message = "An error occurred during processing your request." }
  };

  await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));

  object ValidationError(ValidationException exception)
  {
    httpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
    httpContext.Response.ContentType = "application/json";

    var value = new
    {
      Message = "Request validation failed, please see message(s) above.",
      Errors = exception.Errors.Select(x => new
      {
        FieldName = x.PropertyName,
        Message = x.ErrorMessage,
        YourValue = x.AttemptedValue
      })
    };

    return value;
  }
}));

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseBlazorFrameworkFiles();
app.MapFallbackToFile("/app", "index.html");

app.MapGet("/", () => "running");

app.MapPost("/poll",
  async (ISender sender, [FromBody] CreatePollCommand cmd) =>
    await sender.Send(cmd) switch
    {
      OkResult<string> x => Results.Created($"poll/{x.Value}", x.Value),
    });

app.MapGet("/poll/{id}",
  async (string id, ISender sender) =>
    await sender.Send(new GetPollQuery { PollId = id }) switch
    {
      OkResult<PollModel> x => Results.Ok(x.Value),
      NotFoundResult<string> x => Results.NotFound(x),
      ErrorResult x => Results.BadRequest(x.Message),
    });

app.MapPost("/poll/respond",
  async (RespondCommand command, ISender sender) =>
    await sender.Send(command) switch
    {
      NotFoundResult x => Results.Problem(x.Message, statusCode: (int)HttpStatusCode.NotFound),
      ErrorResult x => Results.Problem(x.Message, statusCode: (int)HttpStatusCode.BadRequest),
      OkResult => Results.Ok()
    });

app.MapGet("/poll/{id}/responds",
  async (string id, ISender sender) =>
    await sender.Send(new GetRespondsQuery() { PollId = id }) switch
    {
      OkResult<RespondsModel> x => Results.Ok(x.Value),
      NotFoundResult => Results.NotFound(),
      ErrorResult x => Results.BadRequest(x.Message)
    });

app.Run();