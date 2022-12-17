using AutoMapper;
using Produtos.CrossCutting.AutoMapper.Profiles;

namespace Produtos.CrossCutting.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration((config) => {
                config.AddProfile(new ModelsMappingProfile());
            });
        }
    }
}
