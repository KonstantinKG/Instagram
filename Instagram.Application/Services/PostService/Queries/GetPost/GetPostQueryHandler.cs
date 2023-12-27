using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Queries.GetPost;

public class GetPostQueryHandler
{
    private readonly IDapperPostRepository _dapperPostRepository;

    public GetPostQueryHandler(IDapperPostRepository dapperUserRepository)
    {
        _dapperPostRepository = dapperUserRepository;
    }
    
    public async Task<ErrorOr<GetPostResult>> Handle(GetPostQuery query, CancellationToken cancellationToken)
    {
        if (await _dapperPostRepository.GetPost(query.Id) is not Post post)
        {
            return Errors.Common.NotFound;
        }
        return new GetPostResult(post);
    }
}