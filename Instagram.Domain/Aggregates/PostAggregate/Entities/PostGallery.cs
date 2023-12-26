﻿using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostGallery : Entity<Guid>
{
    public string File { get; private set; }
    public string? Description { get; private set; }
    public string? Labels { get; private set; }

    private PostGallery(
        Guid id,
        string file,
        string? description,
        string? labels
        )
    : base(id)
    {
        File = file;
        Description = description;
        Labels = labels;
    }
    
    public static PostGallery Create(
        string file,
        string? description,
        string? labels
        )
    {
        return new PostGallery(
            Guid.NewGuid(), 
            file,
            description,
            labels
            );
    }
    
    public static PostGallery Fill(
        Guid id,
        string file,
        string? description,
        string? labels
        )
    {
        return new PostGallery(
            id, 
            file,
            description,
            labels
            );
    }
    
# pragma warning disable CS8618
    private PostGallery()
    {
    }
# pragma warning disable CS8618
}