using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PredicateBuilder
{    
    internal class ReplaceVisitor : ExpressionVisitor
    {
        private readonly Expression _from;
        private readonly Expression _to;
        public ReplaceVisitor(Expression from, Expression to)
        {
            this._from = from;
            this._to = to;
        }
        public override Expression Visit(Expression node) => node == _from ? _to : base.Visit(node);
    }
    internal static class ReplaceVisitorExtensions
    {
        public static Expression Replace(this Expression expression,
       Expression searchEx, Expression replaceEx) => new ReplaceVisitor(searchEx, replaceEx).Visit(expression);
    }
}
