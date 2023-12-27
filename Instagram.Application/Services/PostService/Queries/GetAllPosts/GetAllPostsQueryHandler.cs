using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Services.UserService.Queries.GetAllUsers;
using Instagram.Domain.Aggregates.PostAggregate;
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
            var total = await _dapperPostRepository.GetTotalPosts();
            var pages = total / limit + (total % limit > 0 ? 1 : 0);

            var posts = new List<Post>();
            if (query.Page <= total)
                posts = await _dapperPostRepository.GetPosts(offset, limit);
            
            return new GetAllPostsResult(
                query.Page,
                pages,
                posts
                );
        }
        catch (Exception)
        {
            return Errors.Common.Unexpected;
        }

    }
}