// AutoMapperConfiguration.cs

namespace Northwind.WebApi2Services
{
    using AutoMapper;

    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            //Mapper.CreateMap<Product, ProductListItemDto>();

            Mapper.AssertConfigurationIsValid();
        }
    }
}