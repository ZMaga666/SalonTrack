using AutoMapper;
using SalonTrackApi.DTO;
using SalonTrackApi.Entities;

namespace SalonTrackApi.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserListDto>();
            CreateMap<User, UserEditDto>();
            CreateMap<UserEditDto, User>();
        }
    }
}
