using System;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.Dto;

namespace DM.Services.Forum.Factories
{
    public interface ITopicFactory
    {
        ForumTopic Create(Guid forumId, CreateTopic createTopic);
    }
}