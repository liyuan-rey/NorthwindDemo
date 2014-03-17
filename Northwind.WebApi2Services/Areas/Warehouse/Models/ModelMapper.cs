// ModelMapper.cs

namespace Northwind.WebApi2Services.Areas.Warehouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Dto;
    using EF6Models;

    public class ModelMapper
    {
        #region Category, CategoryListItemDto

        public static Expression<Func<Category, CategoryListItemDto>> Category2CategoryListItemDto =
            c => new CategoryListItemDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Picture = c.Picture
            };

        #endregion

        #region Category, CategoryNewDto

        public static Expression<Func<Category, CategoryNewDto>> Category2CategoryNewDto =
            c => new CategoryNewDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description,
                Picture = c.Picture
            };

        public static Expression<Func<CategoryNewDto, Category>> CategoryNewDto2Category =
            c => new Category
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description,
                Picture = c.Picture
            };

        #endregion

        #region Category, CategoryUpdateDto

        public static Expression<Func<Category, CategoryUpdateDto>> Category2CategoryUpdateDto =
            c => new CategoryUpdateDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description,
                Picture = c.Picture
            };

        public static Expression<Func<CategoryUpdateDto, Category>> CategoryUpdateDto2Category =
            c => new Category
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description,
                Picture = c.Picture
            };

        #endregion

        #region Product, ProductListItemDto

        public static Expression<Func<Product, ProductListItemDto>> Product2ProductListItemDto =
            p => new ProductListItemDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.CategoryName,
                Discontinued = p.Discontinued,
                UnitPrice = p.UnitPrice
            };

        #endregion

        public static TDestination Map<TSource, TDestination>(TSource source, IEnumerable<string> properties)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            var result = Activator.CreateInstance<TDestination>();

            PropertyInfo[] srcProps = source.GetType().GetProperties();
            PropertyInfo[] destProps = result.GetType().GetProperties();

            IList<string> enumerable = properties as IList<string> ?? properties.ToList();
            var mapping = enumerable.Select(p =>
            {
                PropertyInfo srcPropInfo = srcProps.FirstOrDefault(pi => string.Compare(pi.Name, p, true) == 0);
                PropertyInfo destPropInfo = destProps.FirstOrDefault(pi => string.Compare(pi.Name, p, true) == 0);
                if (srcPropInfo != null && destPropInfo != null)
                {
                    return new
                    {
                        propName = p,
                        srcProp = srcPropInfo,
                        destProp = destPropInfo
                    };
                }

                return null;
            });

            foreach (var mi in mapping)
            {
                ParameterExpression param = Expression.Parameter(source.GetType(), "param");

                Expression getValueExp = Expression.Lambda(
                    Expression.Property(param, mi.propName), param);
                var getPropertyValueLambda =
                    (Expression<Func<TSource, object>>) getValueExp;

                MethodInfo setter = mi.destProp.GetSetMethod();

                ParameterExpression paramo = Expression.Parameter(result.GetType(), "param");
                ParameterExpression parami = Expression.Parameter(mi.destProp.PropertyType, "newvalue");
                MethodCallExpression methodCallSetterOfProperty = Expression.Call(paramo, setter, parami);
            }

            return result;
        }
    }
}