using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Community.Authorization;
using DM.Services.Core.Configuration;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using Microsoft.Extensions.Options;

namespace DM.Web.Classic.Views.Profile.Actions
{
    public class ProfileActionsViewModelBuilder : IProfileActionsViewModelBuilder
    {
        private readonly IIntentionManager intentionManager;
        private readonly IIdentityProvider identityProvider;
        private readonly IntegrationSettings integrationSettings;

        public ProfileActionsViewModelBuilder(
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider,
            IOptions<IntegrationSettings> options)
        {
            this.intentionManager = intentionManager;
            this.identityProvider = identityProvider;
            integrationSettings = options.Value;
        }

        public ProfileActionsViewModel Build(GeneralUser user)
        {
            var canEdit = intentionManager.IsAllowed(UserIntention.Edit, user);
            var userRole = identityProvider.Current.User.Role;
            return new ProfileActionsViewModel
            {
                Login = user.Login,
                // CanWriteMessages = intentionsManager.IsAllowed(UserIntention.WriteMessage, user),
                // CanReportUser = intentionsManager.IsAllowed(UserIntention.Report, user),
                // CanReport = intentionsManager.IsAllowed(CommonIntention.Report),
                // CanViewStatistics = intentionsManager.IsAllowed(UserIntention.ViewStatistics, user),
                CanEdit = canEdit,
                // CanLogin = intentionsManager.IsAllowed(CommonIntention.LogAsAnyUser),
                CanModerate = userRole.HasFlag(UserRole.Administrator) ||
                              userRole.HasFlag(UserRole.SeniorModerator),
                // CanInitiateMerge = canEdit && intentionsManager.IsAllowed(UserIntention.InitiateMerge, user),
                // CanCompleteMerge = canEdit && intentionsManager.IsAllowed(UserIntention.CompleteMerge, user),
                UserId = user.UserId,

                AdministrativeUrl = $"{integrationSettings.AdminUrl}/user/{user.UserId}"
            };
        }
    }
}