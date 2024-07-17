using AutoMapper;
using Mango.Service.ShoppingCart.Model;
using Mango.Service.ShoppingCart.Model.Dto;

namespace Mango.Service.ShoppingCart
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMapper() { 
        
            var mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
                //config.CreateMap<ProductDto, Product>();
            });
        return mappingConfig;
        }
    }
}
