using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using Twitter.Clone.Tweets.Models.Domain;

namespace Twitter.Clone.Tweets.Persistance;

internal class AppDbContext : DbContext
{
    public DbSet<Tweet> Tweets; 
    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions):base(dbContextOptions)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tweet>(builder =>
        {
            
            builder.ToCollection("tweets");
        });
          
        base.OnModelCreating(modelBuilder);
    }
}