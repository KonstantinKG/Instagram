using Instagram.Application.Common.Interfaces.Pipeline;

namespace Instagram.Application.Services.PostService.Queries.AllPostCommentChildren;

public class AllPostCommentChildrenQueryPipeline : BasePipeline<AllPostCommentChildrenQuery, AllPostCommentChildrenResult>
{
    public AllPostCommentChildrenQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}