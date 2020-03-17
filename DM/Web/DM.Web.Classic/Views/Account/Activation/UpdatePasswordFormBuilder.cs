using System;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Account.Verification;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Account.Activation
{
    public class UpdatePasswordFormBuilder : IUpdatePasswordFormBuilder
    {
        private readonly ITokenVerificationService tokenVerificationService;
        private readonly IUserViewModelBuilder userViewModelBuilder;

        public UpdatePasswordFormBuilder(
            ITokenVerificationService tokenVerificationService,
            IUserViewModelBuilder userViewModelBuilder)
        {
            this.tokenVerificationService = tokenVerificationService;
            this.userViewModelBuilder = userViewModelBuilder;
        }

        public async Task<UpdatePasswordForm> Build(Guid token) => new UpdatePasswordForm
        {
            Token = token,
            User = userViewModelBuilder.Build(await tokenVerificationService.Verify(token))
        };
    }
}