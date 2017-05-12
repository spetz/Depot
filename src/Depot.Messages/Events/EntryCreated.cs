namespace Depot.Messages.Events
{
    public class EntryCreated : IEvent
    {
        public string Key { get; }

        protected EntryCreated()
        {
        }

        protected EntryCreated(string key)
        {
            Key = key;
        }
    }
}