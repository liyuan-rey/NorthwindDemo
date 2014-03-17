// PropertyChangedEventHandlerExtensions.cs

namespace Northwind.WpfClient.Common
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Reflection;
    using Northwind.Common;

    public static class PropertyChangedEventHandlerExtensions
    {
        private const string InvalidPropertySelectorExpression = @"Not a valid property selector";

        public static void Notify(this PropertyChangedEventHandler propertyChanged,
            Expression<Func<object>> propertySelector)
        {
            ContractUtil.RequiresNotNull(propertySelector, "propertySelector");

            if (propertyChanged == null)
                return;

            var lambda = propertySelector as LambdaExpression;
            MemberExpression memberExpr;
            var body = lambda.Body as UnaryExpression;
            if (body != null)
                memberExpr = body.Operand as MemberExpression;
            else
                memberExpr = lambda.Body as MemberExpression;

            if (memberExpr == null)
                throw new ArgumentException(InvalidPropertySelectorExpression, "propertySelector");

            var constExpr = memberExpr.Expression as ConstantExpression;
            var propInfo = memberExpr.Member as PropertyInfo;

            if (constExpr == null || propInfo == null)
                throw new ArgumentException(InvalidPropertySelectorExpression, "propertySelector");

            foreach (Delegate delegat in propertyChanged.GetInvocationList())
            {
                delegat.DynamicInvoke(new[]
                {
                    constExpr.Value,
                    new PropertyChangedEventArgs(propInfo.Name)
                });
            }
        }
    }
}