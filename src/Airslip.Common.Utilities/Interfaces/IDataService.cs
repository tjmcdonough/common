namespace Airslip.Common.Utilities.Interfaces
{
    public interface IDataService<TListType> : IDataService<string, TListType> { }
    
    public interface IDataService<in TSearchBy, TListType>
    {
        bool TryGetValue(TSearchBy s, out TListType? bankTradingName);
        
        TListType DefaultValue { get; }
    }
}