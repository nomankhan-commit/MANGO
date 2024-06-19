using AutoMapper;
using Mango.Service.ProductApi.Model;
using Mango.Service.ProductApi.Model.Dto;

namespace Mango.Service.ProductApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMapper() { 
        
            var mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<Product, ProductDto>().ReverseMap();
                //config.CreateMap<ProductDto, Product>();
            });
        return mappingConfig;
        }
    }
}
