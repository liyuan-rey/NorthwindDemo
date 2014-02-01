// PropertiesSelectorHelper.cs

namespace Northwind.WpfClient.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class PropertySelectorHelper
    {
        public static string PropertiesSelectorToStrings<T, TResult>(
            Expression<Func<T, TResult>> propertySelector)
        {
            return ((MemberExpression)propertySelector.Body).Member.Name;
        }

        public static MemberExpression String2PropertySelector<T>(T obj, string propertyName)
        {
            return Expression.Property(Expression.Constant(obj), propertyName);
        }
    }
}