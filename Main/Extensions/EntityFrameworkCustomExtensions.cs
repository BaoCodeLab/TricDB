using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 扩展EF框架，实现根据Table名称进行查询
    /// 参考https://stackoverflow.com/questions/48041821/dynamically-access-table-in-ef-core-2-0
    /// </summary>
    public static partial class CustomExtensions
    {
        public static IQueryable Query(this DbContext context, string entityName) =>
            context.Query(context.Model.FindEntityType(entityName).ClrType);

        static readonly MethodInfo SetMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set));

        public static IQueryable Query(this DbContext context, Type entityType) =>
            (IQueryable)SetMethod.MakeGenericMethod(entityType).Invoke(context, null);
    }
}
