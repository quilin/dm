using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Repositories;
using DM.Services.Forum.BusinessProcesses.Common;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Services.Forum.BusinessProcesses.Topics;
using DM.Tests.Core;

namespace DM.Services.Forum.Tests.BusinessProcesses.Topics
{
    public class TopicReadingServiceShould : UnitTestBase
    {
        private readonly TopicReadingService service;

        public TopicReadingServiceShould()
        {
            service = new TopicReadingService(Mock<IIdentityProvider>().Object,
                Mock<IForumReadingService>().Object, Mock<IAccessPolicyConverter>().Object,
                Mock<ITopicRepository>().Object, Mock<IUnreadCountersRepository>().Object);
        }
    }
}