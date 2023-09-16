using AutoMapper;
using Data.Models;
using Data.ViewModels;

namespace BusinessLogic.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserLogin>()
                .ForMember(dst => dst.Username, src => src.MapFrom(A => A.Email))
                .ForMember(dst => dst.Password, src => src.MapFrom(A => A.Password))
                .ReverseMap();

            CreateMap<User, UserRegister>()
                .ForMember(dst => dst.Username, src => src.MapFrom(A => A.Email))
                .ForMember(dst => dst.Password, src => src.MapFrom(A => A.Password))
                .ForMember(dst => dst.Email, src => src.MapFrom(A => A.Email))
                .ForMember(dst => dst.Address, src => src.MapFrom(A => A.Address))
                .ForMember(dst => dst.Gender, src => src.MapFrom(A => A.Gender))
                .ForMember(dst => dst.Phone, src => src.MapFrom(A => A.Phone))
                .ForMember(dst => dst.FirstName, src => src.MapFrom(A => A.FirstName))
                .ForMember(dst => dst.LastName, src => src.MapFrom(A => A.LastName))
                .ReverseMap();
        }
    }
}
