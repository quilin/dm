using System;

namespace DM.Services.DataAccess.Eventing
{
    public class InvokedEvent
    {
        public EventType Type { get; set; }
        public Guid EntityId { get; set; }
    }
}