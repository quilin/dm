using System;
using System.Threading.Tasks;
using DM.Services.Core.Emails;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Dto;
using DM.Services.MessageQueuing.Processing;

namespace DM.Services.Registration.Consumer
{
    /// <summary>
    /// Processor of user registration events
    /// </summary>
    public class UserRegistrationProcessor : IMessageProcessor<InvokedEvent>
    {
        private readonly IRegistrationTokenFactory registrationTokenFactory;
        private readonly IRegistrationTokenRepository registrationTokenRepository;
        private readonly IMailSender mailSender;

        /// <inheritdoc />
        public UserRegistrationProcessor(
            IRegistrationTokenFactory registrationTokenFactory,
            IRegistrationTokenRepository registrationTokenRepository,
            IMailSender mailSender)
        {
            this.registrationTokenFactory = registrationTokenFactory;
            this.registrationTokenRepository = registrationTokenRepository;
            this.mailSender = mailSender;
        }

        /// <inheritdoc />
        public async Task<ProcessResult> Process(InvokedEvent message)
        {
            try
            {
                var token = registrationTokenFactory.Create(message.EntityId);
                await registrationTokenRepository.Add(token);
                var userEmail = await registrationTokenRepository.GetRegisteredUserEmail(message.EntityId);
                await mailSender.Send(userEmail, "Добро пожаловать на DungeonMaster.ru!",
                    $"Вы успешно прошли регистрацию. Осталось только перейти по ссылке и активировать вашу учетную запись! {token.TokenId}");
                // todo: need different policies for different response from SMTP
                return ProcessResult.Success;
            }
            catch (Exception e)
            {
                var text = e.Message;
                return ProcessResult.RetryNeeded;
            }
        }
    }
}