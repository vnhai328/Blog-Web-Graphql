using auth_graphql.Models;
using Microsoft.EntityFrameworkCore;

namespace auth_graphql.Data;

public class MSSqlDbContext : DbContext
{
    public MSSqlDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Post>()
            .HasMany(p => p.Comments)
            .WithOne(p => p.Post!)
            .HasForeignKey(p => p.PostId);

        modelBuilder
            .Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(c => c.Comments)
            .HasForeignKey(c => c.PostId);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Like> Likes { get; set; }
}