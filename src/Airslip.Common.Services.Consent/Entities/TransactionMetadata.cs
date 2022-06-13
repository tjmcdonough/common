namespace Airslip.Common.Services.Consent.Entities
{
    public class TransactionMetadata
    {
        public bool Viewed { get; private set; }

        public long RequestTimestamp { get; private set; }

        public void ChangeToViewed(long timestamp)
        {
            Viewed = true;
            RequestTimestamp = timestamp;
        }
    }
}