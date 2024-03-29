using Microsoft.AspNetCore.Mvc;
using Twitter.Clone.Tweets.Extensions;
using Twitter.Clone.Tweets.Models.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigurMapster();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/Tweet", ( CreateTweetRequest request, CancellationToken cancellationToken) =>
{

});

app.Run();