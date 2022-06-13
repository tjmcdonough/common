using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using System.Collections.Generic;

namespace Airslip.Common.Repository.Interfaces;

public interface IEntityPreProcessEvent<TEntity> : IEntityProcessEvent<TEntity>
    where TEntity : class, IEntity 
{

}