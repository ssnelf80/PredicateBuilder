using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PredicateBuilder
{      
    public class PredicateBuilder<TEntity>
    {          
       
        private Expression<Func<TEntity, bool>>? _lambda = null;       

        public PredicateBuilder<TEntity> And(Expression<Func<TEntity, bool>> lambda)
        {
            if (_lambda == null)
            {
                _lambda = lambda;
                return this;
            }
            var addBody = lambda.Body.Replace(lambda.Parameters[0], _lambda!.Parameters[0]);
            var body = Expression.AndAlso(_lambda.Body, addBody);
            _lambda = Expression.Lambda<Func<TEntity, bool>>(body, _lambda.Parameters[0]);
            return this;
        }              

        public PredicateBuilder<TEntity> Or(Expression<Func<TEntity, bool>> lambda)
        {
            if (_lambda == null)
            {
                _lambda = lambda;
                return this;
            }
            var addBody = lambda.Body.Replace(lambda.Parameters[0], _lambda!.Parameters[0]);
            var body = Expression.OrElse(_lambda.Body, addBody);
            _lambda = Expression.Lambda<Func<TEntity, bool>>(body, _lambda.Parameters[0]);
            return this;
        }
             
        public Expression<Func<TEntity, bool>> GetLambda(bool @default = true) => _lambda ??
            Expression.Lambda<Func<TEntity, bool>>(Expression.Constant(@default), Expression.Parameter(typeof(TEntity)));
    }  
    public static class PredicateBuilderExtension
    {       
        private static Expression<Func<TEntity, bool>> CreateLambda<TEntity, P>
           (this PredicateBuilder<TEntity> builder, Expression<Func<TEntity, P>> action, Operation operation, P value)
        {
            var parameter = Expression.Parameter(typeof(TEntity));
            var body = action.Body.Replace(action.Parameters[0], parameter);
           
            var constant = Expression.Constant(value);
            Expression finalExpression = operation switch
            {               
                Operation.EQUALS => Expression.Equal(body, constant),
                Operation.NOT_EQUALS => Expression.NotEqual(body, constant),
                Operation.LESS_THAN => Expression.LessThan(body, constant),
                Operation.GREATER_THAN => Expression.GreaterThan(body, constant),
                Operation.LESS_THAN_OR_EQUEAL => Expression.LessThanOrEqual(body, constant),
                Operation.GREATER_THAN_OR_EQUEAL => Expression.GreaterThanOrEqual(body, constant),
                _ => throw new NotImplementedException()
            };

            var res = Expression.Lambda<Func<TEntity, bool>>(finalExpression, parameter);
            return res;
        }
        public static PredicateBuilder<TEntity> And<TEntity, P>(this PredicateBuilder<TEntity> builder, Expression<Func<TEntity, P>> action, Operation operation, P value)
        {
            return builder.And(builder.CreateLambda(action, operation, value));
        }

        public static PredicateBuilder<TEntity> Or<TEntity, P>(this PredicateBuilder<TEntity> builder, Expression<Func<TEntity, P>> action, Operation operation, P value)
        {
            return builder.Or(builder.CreateLambda(action, operation, value));
        }
    }
}
