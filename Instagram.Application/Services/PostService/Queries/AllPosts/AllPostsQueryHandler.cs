﻿using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Services.PostService.Queries._Common;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Common.Errors;
using Instagram.Domain.Configurations;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Instagram.Application.Services.PostService.Queries.AllPosts;

public class AllPostsQueryHandler
{
    private readonly AppConfiguration _configuration;
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly ILogger<AllPostsQueryHandler> _logger;

    public AllPostsQueryHandler(
        IOptions<AppConfiguration> options,
        IDapperPostRepository dapperUserRepository,
        ILogger<AllPostsQueryHandler> logger)
    {
        _configuration = options.Value;
        _dapperPostRepository = dapperUserRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<AllResult<Post>>> Handle(AllPostsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var limit = _configuration.Application.PaginationLimit;
            var offset = (query.Page - 1) *  limit;
            var total = await _dapperPostRepository.GetTotalPosts(query.Date);
            var pages = total /  limit + (total %  limit > 0 ? 1 : 0);

            var posts = new List<Post>();
            if (query.Page <= total)
                posts = await _dapperPostRepository.AllPosts(offset,  limit, query.Date);
            
            return new AllResult<Post>(
                query.Page,
                pages,
                total,
                posts
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }
    }
}