using AutoMapper;
using Demo.DAL.Models;
using Demp.PL.ViewModels;

namespace Demp.PL.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();// mean map vice verse
                // here i must add this line if i changed something in the attripute and want the clr to map it with another attripute from the other class
                //.ForMember(D=>D.Name , O=>O.MapFrom(S=>S.Name));
        }
    }
}
