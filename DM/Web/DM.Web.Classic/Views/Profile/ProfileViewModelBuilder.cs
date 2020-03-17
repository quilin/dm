using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Community.Authorization;
using DM.Services.Community.BusinessProcesses.Users.Reading;
using DM.Services.Core.Dto;
using DM.Services.Core.Parsing;
using DM.Web.Classic.Views.Profile.Actions;
using DM.Web.Classic.Views.Profile.EditInfo;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Profile
{
    public class ProfileViewModelBuilder : IProfileViewModelBuilder
    {
        private readonly IUserReadingService userReadingService;
        private readonly IUserViewModelBuilder userViewModelBuilder;
        private readonly IProfileActionsViewModelBuilder profileActionsViewModelBuilder;
        private readonly IBbParserProvider bbParserProvider;
        private readonly IEditInfoFormFactory editInfoFormFactory;
        private readonly IIdentityProvider identityProvider;
        private readonly IIntentionManager intentionManager;

        public ProfileViewModelBuilder(
            IUserReadingService userReadingService,
            IUserViewModelBuilder userViewModelBuilder,
            IProfileActionsViewModelBuilder profileActionsViewModelBuilder,
            IIntentionManager intentionManager,
            IBbParserProvider bbParserProvider,
            IEditInfoFormFactory editInfoFormFactory,
            IIdentityProvider identityProvider)
        {
            this.userReadingService = userReadingService;
            this.userViewModelBuilder = userViewModelBuilder;
            this.profileActionsViewModelBuilder = profileActionsViewModelBuilder;
            this.intentionManager = intentionManager;
            this.bbParserProvider = bbParserProvider;
            this.editInfoFormFactory = editInfoFormFactory;
            this.identityProvider = identityProvider;
        }

        public async Task<ProfileViewModel> Build(string login)
        {
            var identity = identityProvider.Current;
            var userDetails = await userReadingService.GetDetails(string.IsNullOrEmpty(login)
                ? identity.User.Login
                : login);

            var info = bbParserProvider.CurrentInfo.Parse(userDetails.Info);

            var canEdit = intentionManager.IsAllowed(UserIntention.Edit, (GeneralUser) userDetails);

            return new ProfileViewModel
            {
                UserId = userDetails.UserId,
                User = userViewModelBuilder.Build(userDetails),
                RegistrationDate = userDetails.RegistrationDate,
                LastVisitDate = userDetails.LastVisitDate,

                Role = userDetails.Role,

                // VotesCount = ratingService.CountUserVotes(user.UserId),

                // WarningsCount = moderationTimeline.WarningPoints,
                // ActiveBanInfo = activeBanView,
                // LastBanInfo = lastBanView,

                ProfilePictureUrl = userDetails.PictureUrl,
                Status = userDetails.Status,
                Email = userDetails.Email,
                ShowEmail = canEdit,

                Name = userDetails.Name,
                Location = userDetails.Location,
                Icq = userDetails.Icq,
                Skype = userDetails.Skype,

                Info = info.ToHtml(),
                EditInfoForm = editInfoFormFactory.Create(userDetails.UserId, info.ToBb()),

                CanEdit = canEdit,
                CanEditSettings = canEdit,
                ProfileActions = profileActionsViewModelBuilder.Build(userDetails),
                // ReportUserForm = reportUserFormBuilder.Build(user.UserId)
            };
        }
    }
}