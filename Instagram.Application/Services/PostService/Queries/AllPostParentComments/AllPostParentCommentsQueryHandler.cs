﻿using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Instagram.Application.Services.PostService.Queries.AllPostParentComments;

public class AllPostParentCommentsQueryHandler
{
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly ApplicationSettings _applicationSettings;
    private readonly ILogger<AllPostParentCommentsQueryHandler> _logger;

    public AllPostParentCommentsQueryHandler(
        IDapperPostRepository dapperUserRepository,
        IOptions<ApplicationSettings> applicationOptions,
        ILogger<AllPostParentCommentsQueryHandler> logger)
    {
        _dapperPostRepository = dapperUserRepository;
        _logger = logger;
        _applicationSettings = applicationOptions.Value;
    }
    
    public async Task<ErrorOr<AllPostParentCommentsResult>> Handle(AllPostParentCommentsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var limit = _applicationSettings.PaginationLimit;
            var offset = (query.Page - 1) *  limit;
            var total = await _dapperPostRepository.GetTotalPostParentComments(query.PostId);
            var pages = total /  limit + (total %  limit > 0 ? 1 : 0);

            var comments = new List<PostComment>();
            if (query.Page <= total)
                comments = await _dapperPostRepository.AllPostParentComments(query.PostId, offset,  limit);
            
            return new AllPostParentCommentsResult(
                query.Page,
                pages,
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