using MapsterMapper;
using Microsoft.EntityFrameworkCore;
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

app.MapPost("/Tweet", async (AppDbContext appDbContext, 
    IMapper mapper, 
    CreateTweetRequest request, 
    CancellationToken cancellationToken) =>
{
    var entity = mapper.Map<Tweet>(request);
    appDbContext.Set<Tweet>().Add(entity);
    await appDbContext.SaveChangesAsync();
});

app.MapPost("/GetTweets", async (AppDbContext appDbContext, 
    IMapper mapper, 
    CancellationToken cancellationToken) =>
{
    var list = appDbContext.Set<Tweet>().ToList().Select
        (c => mapper.Map<CreateTweetRequest>(c));
    return list;
});

app.Run();