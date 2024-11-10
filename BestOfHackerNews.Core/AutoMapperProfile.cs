﻿namespace BestOfHackerNews.Core;

using System;
using System.Linq;
using AutoMapper;
using BestOfHackerNews.Core.Domain;
using BestOfHackerNews.Core.Dto;

internal class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<StoryApiDto, Story>()
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => MapUnixTime(src.Time)))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => CommentCount(src.Kids)));
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