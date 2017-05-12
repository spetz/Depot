using System;

namespace Depot.Messages.Commands
{
    public class CreateEntry
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
}