using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApiBlueprint.Core.Models.Dtos.Common;

namespace ApiBlueprint.DataAccess.DataManipulation;

public interface IExpressionFilter<TItem>
{
    Expression<Func<TItem, bool>> FilteringExpression { get; set; }
    Expression<Func<TItem, object>> OrderingExpression { get; set; }
    OrderingDirection OrderingDirection { get; set; }
    Task<Page<TItem>> ApplyToQueryable(IQueryable<TItem> source);
}