using Instagram.Application.Common.Interfaces.Pipeline;

namespace Instagram.Application.Services.UserService.Queries.GetAllUsers;

public class GetAllUsersQueryPipeline : BasePipeline<GetAllUsersQuery, GetAllUsersResult>
{
    public GetAllUsersQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}