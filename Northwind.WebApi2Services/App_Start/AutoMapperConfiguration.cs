// AutoMapperConfiguration.cs

namespace Northwind.WebApi2Services
{
    using AutoMapper;
    using Dto;
    using EF6Models;

    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            //var map = Mapper.CreateMap<Product, ProductListItemDto>();
            //map.ForMember(d => d.CategoryName, 
            //    opt => opt.MapFrom(s => s.Category.CategoryName));

            Mapper.AssertConfigurationIsValid();
        }
    }
}