using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Mail.Sender;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Dto;
using DM.Services.MessageQueuing.Processing;

namespace DM.Services.Registration.Consumer.Implementation
{
    /// <inheritdoc />
    public class RegistrationProcessor : IMessageProcessor<InvokedEvent>
    {
        private readonly IRegistrationRepository repository;
        private readonly IMailSender mailSender;

        /// <inheritdoc />
        public RegistrationProcessor(
            IRegistrationRepository repository,
            IMailSender mailSender)
        {
            this.repository = repository;
            this.mailSender = mailSender;
        }
        
        /// <inheritdoc />
        public async Task<ProcessResult> Process(InvokedEvent message)
        {
            if (message.Type != EventType.NewUser)
            {
                return ProcessResult.Fail;
            }

            var viewModel = await repository.Get(message.EntityId);
            var letter = new MailLetter
            {
                Address = viewModel.Email,
                Subject = $"Добро пожаловать на DungeonMaster.ru, {viewModel.Login}!",
                Body = $"{viewModel.Login}, вы успешно прошли регистрацию. Осталось только перейти по ссылке и активировать вашу учетную запись! {viewModel.Token}"
            };
            await mailSender.Send(letter);

            return ProcessResult.Success;
        }
    }
}