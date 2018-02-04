using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace Etupirka.EntityFramework.Portal
{
    /// <summary>
    /// 基于EF的Repository基类
    /// </summary>
    public abstract class EtupirkaRepositoryBase<TDbContext, TEntity, TPrimaryKey> : EfRepositoryBase<TDbContext, TEntity, TPrimaryKey>
       where TEntity : class, IEntity<TPrimaryKey>
       where TDbContext : AbpDbContext
    {
        protected EtupirkaRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class EtupirkaRepositoryBase<TDbContext, TEntity> : EtupirkaRepositoryBase<TDbContext, TEntity, int>
        where TEntity : class, IEntity<int>
        where TDbContext : AbpDbContext
    {
        protected EtupirkaRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
