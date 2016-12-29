using AuthNetCore.Models.Identity;
using AuthNetCore.Models.ViewModels.Account;
using AutoMapper;

namespace AuthNetCore.Models.Mappings
{
    public class ApplicationUserMappings
    {
        public static void RegisterMappings(IMapperConfigurationExpression config)
        {
            config.CreateMap<RegisterVM, ApplicationUser>()
                .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.Cellphone));

            config.CreateMap<ApplicationUser, RegisterVM>()
                .ForMember(d => d.Cellphone, o => o.MapFrom(s => s.PhoneNumber));
        }
    }
}
