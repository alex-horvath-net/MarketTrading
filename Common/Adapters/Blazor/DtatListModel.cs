using System.Linq.Expressions;

namespace Common.Adapters.Blazor {
    public class DataListModel<TEntity> {
        public List<TEntity> Rows { get; set; } = [];
        public List<Expression<Func<TEntity, object>>> Columns { get; set; } = [];

    }
}
