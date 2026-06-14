using AutoMapper;
using Demo.DAL.Models;
using Demp.PL.ViewModels;

namespace Demp.PL.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        }
    }
}
