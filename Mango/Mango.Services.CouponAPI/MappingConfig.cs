﻿using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;

namespace Mango.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMapper() { 
        
            var mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<Coupons, CouponDto>();
                config.CreateMap<CouponDto, Coupons>();
            });
        return mappingConfig;
        }
    }
}
