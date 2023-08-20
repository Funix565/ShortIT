using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ShortIT.Dto;
using ShortIT.Models;

namespace ShortIT.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ShortUrl, ShortUrlDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
            
            CreateMap<ApplicationUser, ApplicationUserDto>();
        }
    }
}
