using Abp.Domain.Entities;
using Abp.EntityFramework;

namespace Etupirka.EntityFramework.Portal.Repositories
{
    public abstract class EtupirkaPortalRepositoryBase<TEntity, TPrimaryKey> : EtupirkaRepositoryBase<EtupirkaPortalDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected EtupirkaPortalRepositoryBase(IDbContextProvider<EtupirkaPortalDbContext> dbContextProvider)
            : base(dbContextProvider)
        { }
    }

    public abstract class EtupirkaPortalRepositoryBase<TEntity> : EtupirkaRepositoryBase<EtupirkaPortalDbContext, TEntity, int>
       where TEntity : class, IEntity<int>
    {
        protected EtupirkaPortalRepositoryBase(IDbContextProvider<EtupirkaPortalDbContext> dbContextProvider)
            : base(dbContextProvider)
        { }
    }
}
