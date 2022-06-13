using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.Handoff.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using System.Threading.Tasks;

namespace Airslip.Common.Services.Consent.Interfaces
{
    public interface IRegisterDataService<TEntity, in TModel>  : IMessageHandoffWorker
        where TModel : class, IModel, IFromDataSource
        where TEntity : class, IEntity, IFromDataSource
    {
        
    }
}