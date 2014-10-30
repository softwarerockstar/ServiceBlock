using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace ServiceBlock.Foundation
{    
    public enum StringFilterOperator
    {
        Contains,
        Equals,
        StartsWith,
        EndsWith,
    }

    [ExcludeFromCodeCoverage]
    public static class DynamicExpressionBuilder
    {
        public static Expression<Func<T, bool>> BuildFilterExpression<T>(string propertyName, object propertyValue, StringFilterOperator op = StringFilterOperator.Equals)
        {
            var parameterExp = Expression.Parameter(typeof(T), typeof(T).ToString());
            
            MemberExpression propertyExp;
            if (propertyName.Contains('.'))
            {
                var propSplit = propertyName.Split('.');
                propertyExp = Expression.PropertyOrField(Expression.PropertyOrField(parameterExp, propSplit[0]), propSplit[1]);
            }
            else
                propertyExp = Expression.PropertyOrField(parameterExp, propertyName);

            switch (op)
            {
                case StringFilterOperator.StartsWith:
                case StringFilterOperator.EndsWith:
                case StringFilterOperator.Contains:
                    {
                        MethodInfo method = typeof(string).GetMethod(op.ToString(), new[] { typeof(string) });
                        var someValue = Expression.Constant(propertyValue, typeof(string));
                        var methodExp = Expression.Call(propertyExp, method, someValue);
                        return Expression.Lambda<Func<T, bool>>(methodExp, parameterExp);
                    }
                case StringFilterOperator.Equals:
                    {
                        var bodyExp = Expression.Equal(propertyExp, Expression.Constant(propertyValue));
                        return Expression.Lambda<Func<T, bool>>(bodyExp, parameterExp);
                    }

            }

            return null;
        }

        public static Func<T, string> GetField<T>(string field)
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(field);
            return
                obj => Convert.ToString(propertyInfo.GetValue(obj, null));
        }

    }
}
