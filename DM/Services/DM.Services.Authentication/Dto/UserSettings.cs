using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Authentication.Dto
{
    public class UserSettings
    {
        public Guid Id { get; set; }
        public string NannyGreetingsMessage { get; set; }
        public ColorScheme ColorScheme { get; set; }

        public int PostsPerPage { get; set; }
        public int CommentsPerPage { get; set; }
        public int TopicsPerPage { get; set; }
        public int MessagesPerPage { get; set; }

        public static readonly UserSettings Default = new UserSettings
        {
            TopicsPerPage = 10,
            CommentsPerPage = 10,
            PostsPerPage = 10,
            MessagesPerPage = 10,
            ColorScheme = ColorScheme.Modern
        };
    }
}