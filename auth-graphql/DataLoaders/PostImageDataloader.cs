using auth_graphql.Models;
using auth_graphql.Services.PostImages;

namespace auth_graphql.DataLoaders;

public class PostImageDataLoader : GroupedDataLoader<int, Image>
{
    private readonly IPostImageRepository _imageRepository;

    public PostImageDataLoader(
        IPostImageRepository imageRepository,
        IBatchScheduler batchScheduler,
        DataLoaderOptions? options = null) : base(batchScheduler, options)
    {
        _imageRepository = imageRepository;
    }

    protected override async Task<ILookup<int, Image>> LoadGroupedBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
    {
        var images = await _imageRepository.GetImagesByPostIds(keys);
        return images.ToLookup(i => i.PostId);
    }
}