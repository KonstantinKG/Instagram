using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostToTag : Entity<long>
{
    public long PostId { get; private set; }
    public long TagId { get; private set; }

    private PostToTag(
        long postId,
        long tagId
        )
    {
        PostId = postId;
        TagId = tagId;
    }
    
    public static PostToTag Create(
        long postId,
        long tagId
        )
    {
        return new PostToTag(
            postId,
            tagId
            );
    }
    
# pragma warning disable CS8618
    private PostToTag()
    {
    }
# pragma warning disable CS8618
}