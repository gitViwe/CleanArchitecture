using AutoMapper;
using Core.Response.Identity;
using Infrastructure.Identity;

namespace Infrastructure.Mapping
{
    /// <summary>
    /// A configuration profile to map <see cref="AppIdentityUser"/> and <see cref="UserResponse"/> using Automapper
    /// </summary>
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // create a two way mapping
            CreateMap<AppIdentityUser, UserResponse>().ReverseMap();
        }
    }
}
