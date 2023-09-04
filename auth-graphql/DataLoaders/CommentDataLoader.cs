using auth_graphql.Models;
using auth_graphql.Services.Comments;

namespace auth_graphql.DataLoaders;

public class CommentDataLoader : GroupedDataLoader<int, Comment>
{
    private readonly ICommentRepository _commentRepository;

    public CommentDataLoader(
        ICommentRepository commentRepository,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options = null) : base(batchScheduler, options)
    {
        _commentRepository = commentRepository;
    }
    protected override async Task<ILookup<int, Comment>> LoadGroupedBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
    {
        var comments = await _commentRepository.GetCommentsByPostId(keys);
        return comments.ToLookup(c => c.PostId);
    }
}