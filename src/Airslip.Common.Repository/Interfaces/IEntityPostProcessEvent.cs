using Airslip.Common.Repository.Types.Interfaces;

namespace Airslip.Common.Repository.Interfaces;

public interface IEntityPostProcessEvent<TEntity> : IEntityProcessEvent<TEntity>
    where TEntity : class, IEntity 
{

}