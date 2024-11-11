namespace BestOfHackerNews.Core;

using System;
using AutoMapper;
using BestOfHackerNews.Core.Domain;
using BestOfHackerNews.Core.Dto;

/// <summary>
/// Maps Hacker News API DTOs to Domain objects.
/// </summary>
internal class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<StoryApiDto, Story>()
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => MapUnixTime(src.Time)))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => CommentCount(src.Kids)))
            .ForMember(dest => dest.URI, opt => opt.MapFrom(src => src.URL))
            .ForMember(dest => dest.PostedBy, opt => opt.MapFrom(src => src.By));
    }

    private DateTime MapUnixTime(int unixTime)
    {
        try
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        }
        catch
        {
            return DateTime.MinValue;
        }
    }

    private int CommentCount(int[] comments)
    {
        return comments is null ? 0 : comments.Length;
    }
}
