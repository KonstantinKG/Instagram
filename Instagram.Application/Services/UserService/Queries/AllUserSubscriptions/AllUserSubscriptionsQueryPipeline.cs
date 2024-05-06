using Instagram.Application.Common.Interfaces.Pipeline;
using Instagram.Application.Services.PostService.Queries._Common;
using Instagram.Domain.Aggregates.UserAggregate;

namespace Instagram.Application.Services.UserService.Queries.AllUserSubscriptions;

public class AllUserSubscriptionsQueryPipeline : BasePipeline<AllUserSubscriptionsQuery, AllResult<User>>
{
    public AllUserSubscriptionsQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}