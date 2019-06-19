using DM.Web.Classic.Views.Shared.Warnings.CreateWarning;

namespace DM.Web.Classic.Dto
{
    public class CreateWarningModelConverter : ICreateWarningModelConverter
    {
        private readonly IUserService userService;

        public CreateWarningModelConverter(IUserService userService)
        {
            this.userService = userService;
        }

        public CreateWarningModel Convert(CreateWarningForm form)
        {
            return new CreateWarningModel
            {
                WarnedUserId = userService.Find(form.UserName).UserId,
                WarnedEntityId = form.EntityId,
                Points = form.Points,
                Comment = form.Comment
            };
        }
    }
}