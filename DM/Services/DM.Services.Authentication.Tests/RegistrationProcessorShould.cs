using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Mail.Sender;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Dto;
using DM.Services.Registration.Consumer.Implementation;
using DM.Tests.Core;
using FluentAssertions;
using Moq;
using Xunit;

namespace DM.Services.Authentication.Tests
{
    public class RegistrationProcessorShould : UnitTestBase
    {
        private readonly Mock<IRegistrationRepository> registrationRepository;
        private readonly Mock<IMailSender> mailSender;
        private readonly RegistrationProcessor processor;

        public RegistrationProcessorShould()
        {
            registrationRepository = Mock<IRegistrationRepository>();
            mailSender = Mock<IMailSender>();
            processor = new RegistrationProcessor(registrationRepository.Object, mailSender.Object);
        }

        [Fact]
        public async Task FailWhenIncorrectEvent()
        {
            var actual = await processor.Process(new InvokedEvent
            {
                EntityId = Guid.NewGuid(),
                Type = EventType.Unknown
            });
            actual.Should().Be(ProcessResult.Fail);
        }

        [Fact]
        public async Task SendLetterWithTokenAndLogin()
        {
            var token = "1648ae6f-f512-4339-873b-fb8b4a93c2b0";
            var userId = Guid.NewGuid();
            var mailViewModel = new RegistrationMailViewModel
            {
                Email = "email",
                Login = "MyLogin",
                Token = Guid.Parse(token)
            };
            registrationRepository.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(mailViewModel);
            mailSender.Setup(s => s.Send(It.IsAny<MailLetter>())).Returns(Task.CompletedTask);

            var actual = await processor.Process(new InvokedEvent
            {
                EntityId = userId,
                Type = EventType.NewUser
            });

            actual.Should().Be(ProcessResult.Success);
            mailSender.Verify(s => s.Send(It.Is<MailLetter>(l =>
                l.Address == "email" &&
                l.Subject.Contains("MyLogin") &&
                l.Body.Contains(token))), Times.Once);
            registrationRepository.Verify(r => r.Get(userId), Times.Once);
        }
    }
}