using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Services.UserService.Queries.GetAllUsers;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Queries.GetAllPosts;

public class GetAllPostsQueryHandler
{
    private readonly IDapperPostRepository _dapperPostRepository;

    public GetAllPostsQueryHandler(IDapperPostRepository dapperUserRepository)
    {
        _dapperPostRepository = dapperUserRepository;
    }
    
    public async Task<ErrorOr<GetAllPostsResult>> Handle(GetAllPostsQuery query,  CancellationToken cancellationToken)
    {
        try
        {
            const int limit = 15;
            var offset = (query.Page - 1) * limit;
            var posts = await _dapperPostRepository.GetPosts(offset, limit);
            return new GetAllPostsResult(posts);
        }
        catch (Exception)
        {
            return Errors.Common.Unexpected;
        }

    }
}