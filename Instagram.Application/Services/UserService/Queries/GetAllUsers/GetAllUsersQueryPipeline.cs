using Instagram.Application.Common.Interfaces.Pipeline;
using Instagram.Application.Services.PostService.Queries._Common;
using Instagram.Domain.Aggregates.UserAggregate;

namespace Instagram.Application.Services.UserService.Queries.GetAllUsers;

public class GetAllUsersQueryPipeline : BasePipeline<GetAllUsersQuery, AllResult<User>>
{
    public GetAllUsersQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}