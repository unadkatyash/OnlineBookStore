using AutoMapper;
using OnlineBookStore.Business.ViewModels.User;

namespace OnlineBookStore.Business.ViewModels
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<OnlineBookStore.Database.Models.User, UserDetails>();
        }
    }
}
