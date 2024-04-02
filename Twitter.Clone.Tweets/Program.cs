using System.Threading.Channels;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Twitter.Clone.Tweets.BackgroundServices.Channel;
using Twitter.Clone.Tweets.Extensions;
using Twitter.Clone.Tweets.Models.Contracts;
using Twitter.Clone.Tweets.Models.Domain;
using Twitter.Clone.Tweets.Models.Setting;
using Twitter.Clone.Tweets.Persistance;
using Twitter.Clone.Tweets.Principles;
using Twitter.Clone.Tweets.Principles.Interface;

var builder = WebApplication.CreateBuilder(args);

var mongoDbSetting = builder.Configuration.GetSection("MongoDBSetting").Get<MongoDBSetting>();

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseMongoDB(mongoDbSetting.AtlasURI, mongoDbSetting.DatabaseName);
});

builder.Services.ConfigurMapster();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserPrinciple, UserPrinciple>(sp =>
{
    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();

    var httpContext = httpContextAccessor.HttpContext;

    return new UserPrinciple(httpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? string.Empty);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
    services.AddHostedService<ReaderBackgroundService>();
    services.AddSingleton(Channel.CreateUnbounded<CreateTweetContext>());
}).Build();


var channel = host.Services.GetRequiredService<Channel<CreateTweetContext>>();

app.MapPost("/Tweet", async (AppDbContext appDbContext,
    IMapper mapper,
    IUserPrinciple userPrinciple,
    CreateTweetRequest request,
    CancellationToken cancellationToken) =>
{
    var entity = mapper.Map<Tweet>(request);
    appDbContext.Set<Tweet>().Add(entity);
    await appDbContext.SaveChangesAsync();

    await channel.Writer.WriteAsync(new CreateTweetContext(userPrinciple.IpAddress, entity.Content));
});
app.MapPost("/GetTweets", async (AppDbContext appDbContext,
    IMapper mapper,
    CancellationToken cancellationToken) =>
{
    var tweets = appDbContext.Set<Tweet>().ToList()
        .Select(c => mapper.Map<GetTweetRequest>(c));


    return tweets;
});

app.MapPost("/GetTweetByUserId", async (Guid userId, AppDbContext appDbContext,
    IMapper mapper,
    CancellationToken cancellationToken) =>
{
    var tweets = appDbContext.Set<Tweet>()
        .Where(c => c.UserId == userId).ToList()
        .Select(c => mapper.Map<CreateTweetRequest>(c));


    return tweets;
});

app.MapPost("/GetTweetById", async (string id, AppDbContext appDbContext,
    IMapper mapper,
    CancellationToken cancellationToken) =>
{
    ObjectId.Parse(id);
    var objectId = ObjectId.Parse(id);
    var tweets = appDbContext.Set<Tweet>()
        .Where(c => c.Id == objectId).ToList()
        .Select(c => mapper.Map<CreateTweetRequest>(c));


    return tweets;
});

app.Run();