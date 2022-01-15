using AutoMapper;
using Core.Response.Identity;
using Infrastructure.Identity;

namespace Infrastructure.Mapping
{
    /// <summary>
    /// A configuration profile to map <see cref="AppIdentityRole"/> and <see cref="RoleResponse"/> using Automapper
    /// </summary>
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            // create a two way mapping
            CreateMap<AppIdentityRole, RoleResponse>().ReverseMap();
        }
    }
}
