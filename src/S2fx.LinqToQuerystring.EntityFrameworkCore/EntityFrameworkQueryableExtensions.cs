namespace LinqToQuerystring.EntityFrameworkCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using Microsoft.EntityFrameworkCore.Query.Internal;

    public static class EntityFrameworkQueryableExtensions
    {
        internal static readonly MethodInfo GenericIncludeMethodInfo
            = typeof(Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions)
                .GetTypeInfo().GetDeclaredMethods(nameof(Include))
                .Single(
                    mi => mi.GetParameters().Any(
                        pi => pi.Name == "navigationPropertyPath" && pi.ParameterType == typeof(string)));


        public static IQueryable Include(this IQueryable source, string navigationPropertyPath)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrEmpty(navigationPropertyPath))
            {
                throw new ArgumentNullException(nameof(navigationPropertyPath));
            }

            return source.Provider is EntityQueryProvider ?
                source.Provider.CreateQuery(
                    Expression.Call(
                        instance: null,
                        method: GenericIncludeMethodInfo.MakeGenericMethod(source.ElementType),
                        arg0: source.Expression,
                        arg1: Expression.Constant(navigationPropertyPath))
                ) : source;
        }

    }
}
