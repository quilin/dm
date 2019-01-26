using System;
using DM.Services.DataAccess.MongoIntegration;

namespace DM.Services.DataAccess.BusinessObjects.Games.Posts
{
    [MongoCollectionName("Dice")]
    public class DiceRoll
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsAdditional { get; set; }
        public bool IsHidden { get; set; }
        public bool IsFair { get; set; }

        public int ThrowsCount { get; set; }
        public int EdgesCount { get; set; }
        public int? BlastCount { get; set; }
        public int Bonus { get; set; }

        public string Commentary { get; set; }
        public RollResult[] Result { get; set; }
    }

    public class RollResult
    {
        public int Value { get; set; }
        public bool IsCritical { get; set; }
        public bool IsBlasted { get; set; }
    }
}