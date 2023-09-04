using auth_graphql.Data;
using auth_graphql.Models;
using Microsoft.EntityFrameworkCore;

namespace auth_graphql.Services.PostImages;

public class PostImageRepository : IPostImageRepository
{
    private readonly IDbContextFactory<MSSqlDbContext> _contextFactory;

    public PostImageRepository(IDbContextFactory<MSSqlDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
     public async Task<Image> GetById(int id)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Images.FirstOrDefaultAsync(i => i.Id == id);
        }
    }

    public async Task<IEnumerable<Image>> GetImagesByPostIds(IReadOnlyList<int> postIds)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Images
                .Where(i => postIds.Contains(i.PostId))
                .ToListAsync();
        }
    }
    public async Task<Image> Create(Image image)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            await context.Images.AddAsync(image);
            await context.SaveChangesAsync();

            return image;
        }
    }

    public async Task<bool> Delete(int id)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            var image = await context.Images.FirstOrDefaultAsync(i => i.Id == id);
            if(image == null)
            {
                throw new GraphQLException(new Error("Image not found", "IMAGE_NOT_FOUND"));
            }
            context.Images.Remove(image);
            return await context.SaveChangesAsync() > 0;
        }
    }

}