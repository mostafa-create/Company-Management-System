using AutoMapper;
using Demp.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Demp.PL.MappingProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleViewModel>().
                ForMember(D => D.RoleName, O=>O.MapFrom(S => S.Name)).ReverseMap();
        }
    }
}
