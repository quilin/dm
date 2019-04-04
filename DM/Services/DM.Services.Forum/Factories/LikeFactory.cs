using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.Forum.Factories
{
    /// <inheritdoc />
    public class LikeFactory : ILikeFactory
    {
        private readonly IGuidFactory guidFactory;

        /// <inheritdoc />
        public LikeFactory(
            IGuidFactory guidFactory)
        {
            this.guidFactory = guidFactory;
        }
        
        /// <inheritdoc />
        public Like Create(Guid entityId, Guid userId)
        {
            return new Like
            {
                LikeId = guidFactory.Create(),
                UserId = userId,
                EntityId = entityId
            };
        }
    }
}