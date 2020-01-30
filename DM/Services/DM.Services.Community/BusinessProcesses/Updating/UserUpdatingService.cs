using System.Threading.Tasks;
using DM.Services.Community.Dto;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Updating
{
    /// <inheritdoc />
    public class UserUpdatingService : IUserUpdatingService
    {
        private readonly IValidator<UpdateUser> validator;

        /// <inheritdoc />
        public UserUpdatingService(
            IValidator<UpdateUser> validator)
        {
            this.validator = validator;
        }
    
        /// <inheritdoc />
        public async Task<UserDetails> Update(UpdateUser updateUser)
        {
            await validator.ValidateAndThrowAsync(updateUser);
            return null;
        }
    }
}