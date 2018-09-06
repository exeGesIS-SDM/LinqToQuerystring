﻿namespace LinqToQuerystring.TreeNodes.Comparisons
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Antlr.Runtime;

    using LinqToQuerystring.TreeNodes.Base;
    using DataTypes;
    public class EqualsNode : TwoChildNode
    {
        public EqualsNode(Type inputType, IToken payload, TreeNodeFactory treeNodeFactory)
            : base(inputType, payload, treeNodeFactory)
        {
        }

        public override Expression BuildLinqExpression(IQueryable query, Expression expression, Expression item = null)
        {
            var leftExpression = this.LeftNode.BuildLinqExpression(query, expression, item);
            var rightExpression = this.RightNode.BuildLinqExpressionWithComparison(query, expression, item, leftExpression);

            // Nasty workaround to avoid comparison of Aggregate functions to true or false which breaks Entity framework
            if (leftExpression.Type == typeof(bool) && rightExpression.Type == typeof(bool) && rightExpression is ConstantExpression)
            {
                if ((bool)(rightExpression as ConstantExpression).Value)
                {
                    return leftExpression;
                }

                return Expression.Not(leftExpression);
            }
            
            if(leftExpression.Type.IsEnum && rightExpression.Type == typeof(string))
            {
                Type type = leftExpression.GetType();
                
            }

            if (rightExpression.Type == typeof(bool) && leftExpression.Type == typeof(bool)
                && leftExpression is ConstantExpression)
            {
                if ((bool)(leftExpression as ConstantExpression).Value)
                {
                    return rightExpression;
                }

                return Expression.Not(rightExpression);
            }

            NormalizeTypes(ref leftExpression, ref rightExpression);

            return ApplyEnsuringNullablesHaveValues(Expression.Equal, leftExpression, rightExpression);
        }
    }
}