using System;
using System.Collections.Generic;
using DM.Services.DataAccess.MongoIntegration;

namespace DM.Services.DataAccess.BusinessObjects.Fora
{
    [MongoCollectionName("Polls")]
    public class Poll
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Text { get; set; }
        public PollOption[] Options { get; set; }
    }

    public class PollOption
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public List<Guid> UserIds { get; set; }
    }
}