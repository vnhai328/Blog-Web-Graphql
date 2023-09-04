using auth_graphql.Models;
using auth_graphql.Services.Likes;

namespace auth_graphql.DataLoaders;

public class PostLikeDataLoader : GroupedDataLoader<int, Like>
{
    private readonly ILikeRepository _likeRepository;

    public PostLikeDataLoader(
        ILikeRepository likeRepository,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options = null) : base(batchScheduler, options)
    {
        _likeRepository = likeRepository;
    }

    protected override async Task<ILookup<int, Like>> LoadGroupedBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
    {
        var postLikes = await _likeRepository.GetLikeByPostId(keys);
        return postLikes.ToLookup(p => p.Like_Id);
    }
}