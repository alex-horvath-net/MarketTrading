using System.Linq.Expressions;

namespace Infrastructure.Adapters.Blazor;
public class TableModel<TEntity> {
    public List<TEntity> Rows { get; set; } = [];
    public List<Expression<Func<TEntity, object>>> Columns { get; set; } = [];

}
 