﻿using AutoMapper;
using WebApi.Model;
using WebApi.Model.DTO;

namespace WebApi
{
    public class MappingConfig : Profile
    {

        public MappingConfig()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();

            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<Product, ProductUpdateDTO>().ReverseMap();

            CreateMap<NumberProduct, NumberProductDTO>().ReverseMap();
            CreateMap<NumberProduct,NumberProductCreateDTO>().ReverseMap();
            CreateMap<NumberProduct,NumberProductUpdateDTO>().ReverseMap();
        }
    }
}
