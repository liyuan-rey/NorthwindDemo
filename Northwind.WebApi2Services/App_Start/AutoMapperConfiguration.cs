// AutoMapperConfiguration.cs

namespace Northwind.WebApi2Services
{
    using System;
    using AutoMapper;

    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.AssertConfigurationIsValid();
        }
    }
}