using System;

namespace Depot.Services.Entries.Models
{
    public class Entry
    {
        public string Key { get; protected set; }
        public object Value { get; protected set; }
        public DateTime CreatedAt { get; protected set; } 

        protected Entry()
        {
        }

        public Entry(string key, object value)
        {
            Key = key;
            Value = value;
            CreatedAt = DateTime.UtcNow;
        }          
    }
}