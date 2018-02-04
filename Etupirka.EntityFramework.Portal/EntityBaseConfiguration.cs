using System.Data.Entity.ModelConfiguration;
using Abp.Domain.Entities;

namespace Etupirka.EntityFramework.Portal
{
    /// <summary>
    /// EF实体Mapping配置
    /// </summary>
    public class EntityBaseConfiguration<TEntity, TPrimaryKey> : EntityTypeConfiguration<TEntity>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public EntityBaseConfiguration()
        {
            HasKey(e => e.Id);
        }
    }
}
