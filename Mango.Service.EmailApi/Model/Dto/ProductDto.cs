﻿using System.ComponentModel.DataAnnotations;

namespace Mango.Service.EmailApi.Model.Dto
{
    public class ProductDto
    {

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public int Count { get; set; }

    }
}