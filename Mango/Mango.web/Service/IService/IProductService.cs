﻿using Mango.web.Models.Dto;

namespace Mango.web.Service.IService
{
    public interface IProductService
    {

        Task<ResponseDto?> GetProductAsync(int productid);
        Task<ResponseDto?> GetAllProductAsync();
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> CreateProductAsync(ProductDto product);
        Task<ResponseDto?> UpdateProductAsync(ProductDto product);
        Task<ResponseDto?> DeleteProductAsync(int id);
       
    }
}
