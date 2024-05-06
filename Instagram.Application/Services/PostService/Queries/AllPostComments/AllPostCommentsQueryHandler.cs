﻿using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Services.PostService.Queries._Common;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Common.Errors;
using Instagram.Domain.Configurations;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Instagram.Application.Services.PostService.Queries.AllPostComments;

public class AllPostCommentsQueryHandler
{
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly AppConfiguration _configuration;
    private readonly ILogger<AllPostCommentsQueryHandler> _logger;

    public AllPostCommentsQueryHandler(
        IOptions<AppConfiguration> options,
        IDapperPostRepository dapperUserRepository,
        ILogger<AllPostCommentsQueryHandler> logger)
    {
        _configuration = options.Value;
        _dapperPostRepository = dapperUserRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<AllResult<PostComment>>> Handle(AllPostCommentsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var limit = _configuration.Application.PaginationLimit;
            var offset = (query.Page - 1) *  limit;
            var total = await _dapperPostRepository.GetTotalPostParentComments(query.PostId);
            var pages = total /  limit + (total %  limit > 0 ? 1 : 0);

            var comments = new List<PostComment>();
            if (query.Page <= total)
                comments = await _dapperPostRepository.AllPostComments(query.PostId, offset,  limit);
            
            return new AllResult<PostComment>(
                query.Page,
                pages,
                total,
                comments
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }
    }
}