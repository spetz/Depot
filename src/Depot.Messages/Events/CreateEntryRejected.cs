namespace Depot.Messages.Events
{
    public class CreateEntryRejected: IEvent
    {
        public string Key { get; }
        public string Reason { get; }

        protected CreateEntryRejected()
        {
        }

        public CreateEntryRejected(string key, string reason)
        {
            Key = key;
            Reason = reason;
        }
    }
}