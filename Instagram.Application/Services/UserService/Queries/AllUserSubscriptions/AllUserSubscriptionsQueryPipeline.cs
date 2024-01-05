using Instagram.Application.Common.Interfaces.Pipeline;

namespace Instagram.Application.Services.UserService.Queries.AllUserSubscriptions;

public class AllUserSubscriptionsQueryPipeline : BasePipeline<AllUserSubscriptionsQuery, AllUserSubscriptionsResult>
{
    public AllUserSubscriptionsQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}