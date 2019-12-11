using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Rooms.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using FluentValidation;

namespace DM.Services.Gaming.BusinessProcesses.Pending.Creating
{
    /// <inheritdoc />
    public class PendingPostCreatingService : IPendingPostCreatingService
    {
        private readonly IValidator<CreatePendingPost> validator;
        private readonly IRoomReadingService roomReadingService;
        private readonly IIntentionManager intentionManager;

        /// <inheritdoc />
        public PendingPostCreatingService(
            IValidator<CreatePendingPost> validator,
            IRoomReadingService roomReadingService,
            IIntentionManager intentionManager)
        {
            this.validator = validator;
            this.roomReadingService = roomReadingService;
            this.intentionManager = intentionManager;
        }
        
        /// <inheritdoc />
        public async Task<PendingPost> Create(CreatePendingPost createPendingPost)
        {
            var room = await roomReadingService.Get(createPendingPost.RoomId);
            await intentionManager.ThrowIfForbidden(RoomIntention.CreatePendingPost);
        }
    }
}