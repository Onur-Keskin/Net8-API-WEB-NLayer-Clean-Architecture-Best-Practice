using App.Application.Features.User.Dto;
using App.Application.Features.User.Update;
using AutoMapper;

namespace App.Application.Features.User
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            //CreateMap<Domain.Entities.User, UserDto>().ReverseMap();

            CreateMap<Domain.Entities.User, UserDto>()
                    .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id)) // BaseEntity'deki Id'yi mapliyor
                    .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                    .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
                    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role)).ReverseMap();

            CreateMap<Domain.Entities.User, UserProfileDto>().ReverseMap();

            CreateMap<UpdateUserRequest, Domain.Entities.User>().ReverseMap();
        }
    }
}
