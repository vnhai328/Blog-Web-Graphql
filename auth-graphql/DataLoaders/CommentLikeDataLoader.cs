using auth_graphql.Models;
using auth_graphql.Services.Comments;
using auth_graphql.Services.Likes;

namespace auth_graphql.DataLoaders;

public class CommentLikeDataLoader : GroupedDataLoader<int, Like>
{
    private readonly ILikeRepository _likeRepository;

    public CommentLikeDataLoader(
        ILikeRepository likeRepository,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options = null) : base(batchScheduler, options)
    {
        _likeRepository = likeRepository;
    }

    protected override async Task<ILookup<int, Like>> LoadGroupedBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
    {
        var commentLikes = await _likeRepository.GetLikeByCommentId(keys);
        return commentLikes.ToLookup(c => c.Like_Id);
    }
}