namespace Airslip.Integrations.Banking.Types.Enums
{
    public enum BankingIsoFamilyCodes
    {
        UNSUPPORTED,
        RCDT,//Received Credit Transfers
        ICDT,//Issued Credit Transfers
        RCCN,//Received Cash Concentration Transactions
        ICCN,//Issued Cash Concentration Transactions
        RDDT,//Received Direct Debits
        IDDT,//Issued Direct Debits
        RCHQ,//Received Cheques
        ICHQ,//Issued Cheques
        CCRD,//Customer Card Transactions
        MCRD,//Merchant Card Transactions
        LBOX,//Lockbox Transactions
        CNTR,//Counter Transactions
        DRFT,//Drafts/BillOfOrders
        RRCT,//Received Real Time Credit Transfer
        IRCT,//Issued Real Time Credit Transfer
        CAPL,//Cash Pooling
        ACCB,//Account Balancing
        OCRD,//OTC Derivatives – Credit Derivatives
        OIRT,//OTC Derivatives – Interest Rates
        OEQT,//OTC Derivatives – Equity
        OBND,//OTC Derivatives – Bonds
        OSED,//OTC Derivatives – Structured Exotic Derivatives
        OSWP,//OTC Derivatives – Swaps
        LFUT,//Listed Derivatives – Futures
        LOPT,//Listed Derivatives – Options
        FTLN,//Fixed Term Loans
        NTLN,//Notice Loans
        FTDP,//Fixed Term Deposits
        NTDP,//Notice Deposits
        MGLN,//Mortgage Loans
        CSLN,//Consumer Loans
        SYDN,//Syndications
        FWRD,//Forwards
        SWAP,//Swaps
        NDFX,//Non Deliverable
        SPOT,//Spots
        FTUR,//Futures
        OPTN,//Options
        DLVR,//Delivery
        LOCT,//Stand-By Letter Of Credit
        DCCT,//Documentary Credit
        CLNC,//Clean Collection
        DOCC,//Documentary Collection
        GUAR,//Guarantees
        SETT,//Trade, Clearing and Settlement
        NSET,//Non Settled
        BLOC,//Blocked Transactions
        OTHB,//CSD Blocked Transactions
        COLL,//Collateral Management
        CORP,//Corporate Action
        CUST,//Custody
        COLC,//Custody Collection
        LACK,//Lack
        CASH,//Miscellaneous Securities Operations
        OPCL,//Not available
        ACOP,//Additional Miscellaneous Credit Operations
        ADOP,//Additional Miscellaneous Debit Operations
        NTAV,
        MDOP
    }
}