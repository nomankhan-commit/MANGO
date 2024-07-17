using AutoMapper;
using Mango.Service.OrderApi.Model;
using Mango.Service.OrderApi.Model.Dto;

namespace Mango.Service.OrderApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMapper() { 
        
            var mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(dest => dest.CartTotal, u => u.MapFrom(src => src.OrderTotal))
                .ReverseMap();

                config.CreateMap<CartDetailsDto, OrderDetailsDto>()
                .ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.ProductDto.ProductName))
                .ForMember(dest => dest.ProductPrice, u => u.MapFrom(src => src.ProductDto.Price));

                config.CreateMap<OrderDetailsDto, CartDetailsDto>();
                config.CreateMap<OrderDetailsDto, OrderDetails>();

                config.CreateMap<OrderHeader, OrderHeaderDto>();
                config.CreateMap<OrderHeaderDto,OrderHeader>();
            });
        return mappingConfig;
        }
    }
}
