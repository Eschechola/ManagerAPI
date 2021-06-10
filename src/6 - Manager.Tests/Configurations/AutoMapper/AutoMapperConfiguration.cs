using AutoMapper;
using Manager.API.ViewModes;
using Manager.Domain.Entities;
using Manager.Services.DTO;

namespace Manager.Tests.Configurations.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static IMapper GetConfiguration()
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>()
                    .ReverseMap();

                cfg.CreateMap<CreateUserViewModel, UserDTO>()
                    .ReverseMap();

                cfg.CreateMap<UpdateUserViewModel, UserDTO>()
                    .ReverseMap();
            });

            return autoMapperConfig.CreateMapper();
        }
    }
}
