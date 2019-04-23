using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Emails;
using DM.Services.MessageQueuing.Dto;

namespace DM.Services.Mail.Consumer.Processes.Registration
{
    /// <summary>
    /// Processor of user registration events
    /// </summary>
    public class RegistrationProcessor : BaseMailProcessor
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
        protected override EventType EventType => EventType.NewUser;

        /// <inheritdoc />
        public override async Task Process(InvokedEvent message)
        {
            var viewModel = await repository.Get(message.EntityId);
            await mailSender.Send(viewModel.Email,
                $"Добро пожаловать на DungeonMaster.ru, {viewModel.Login}!",
                $"{viewModel.Login}, вы успешно прошли регистрацию. Осталось только перейти по ссылке и активировать вашу учетную запись! {viewModel.Token}");
        }
    }
}