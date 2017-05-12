using System;

namespace Depot.Messages.Commands
{
    public class CreateEntry : ICommand
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
}