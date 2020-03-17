using System;
using System.Collections.Generic;
using System.Linq;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.Core.Parsing;
using DM.Web.Classic.ViewComponents.Shared.HumanDate;

namespace DM.Web.Classic.Views.Profile.Settings
{
    public class UserSettingsFormBuilder : IUserSettingsFormBuilder
    {
        private readonly IIdentityProvider identityProvider;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IBbParserProvider bbParserProvider;
        private readonly ITimezoneInfoProvider timezoneInfoProvider;

        public UserSettingsFormBuilder(
            IIdentityProvider identityProvider,
            IDateTimeProvider dateTimeProvider,
            IBbParserProvider bbParserProvider,
            ITimezoneInfoProvider timezoneInfoProvider
        )
        {
            this.identityProvider = identityProvider;
            this.dateTimeProvider = dateTimeProvider;
            this.bbParserProvider = bbParserProvider;
            this.timezoneInfoProvider = timezoneInfoProvider;
        }

        public UserSettingsForm Build()
        {
            var settings = identityProvider.Current.Settings;
            var user = identityProvider.Current.User;

            var timeZones = TimeZoneInfo.GetSystemTimeZones();
            var currentTimeZone = timezoneInfoProvider.Current ??
                                  timeZones.FirstOrDefault(z => z.GetUtcOffset(dateTimeProvider.Now) == timezoneInfoProvider.Delta) ??
                                  TimeZoneInfo.Local;
            var timezonesDictionary = new Dictionary<string, string> {{string.Empty, "Выбирать автоматически"}};
            foreach (var timeZone in timeZones)
            {
                timezonesDictionary.Add(timeZone.Id, timeZone.DisplayName);
            }

            return new UserSettingsForm
            {
                PostsPerPage = settings.Paging.PostsPerPage,
                CommentsPerPage = settings.Paging.CommentsPerPage,
                TopicsPerPage = settings.Paging.TopicsPerPage,
                MessagesPerPage = settings.Paging.MessagesPerPage,
                EntitiesPerPage = settings.Paging.EntitiesPerPage,
                CanEditNurseGreetingsMessage = user.Role.HasFlag(UserRole.NannyModerator),
                NurseGreetingsMessage = settings.NannyGreetingsMessage,
                ColorSchema = settings.ColorSchema,
                RatingDisabled = user.RatingDisabled,

                TimezonesList = timezonesDictionary,
                Timezone = currentTimeZone.Id,

                Parser = bbParserProvider.CurrentCommon
            };
        }
    }
}