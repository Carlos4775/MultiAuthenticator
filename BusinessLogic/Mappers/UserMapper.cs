using AutoMapper;
using Data.Models;
using Data.ViewModels;

namespace BusinessLogic.Mappers
{
    internal class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserLogin>()
                .ForMember(dst => dst.Email, src => src.MapFrom(A => A.Email))
                .ForMember(dst => dst.Password, src => src.MapFrom(A => A.Password))
                .ForMember(dst => dst.FirstName, src => src.MapFrom(A => A.FirstName))
                .ForMember(dst => dst.LastName, src => src.MapFrom(A => A.LastName))
                .ForMember(dst => dst.Address, src => src.MapFrom(A => A.Address))
                .ForMember(dst => dst.Gender, src => src.MapFrom(A => A.Gender))
                .ReverseMap();
        }
    }
}
