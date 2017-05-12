namespace Depot.Messages.Events
{
    public class EntryCreated : IEvent
    {
        public string Key { get; }

        protected EntryCreated()
        {
        }

        public EntryCreated(string key)
        {
            Key = key;
        }
    }
}