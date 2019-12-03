using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.CharactersList
{
    public interface ICharactersListViewModelBuilder
    {
        Task<CharactersListViewModel> Build(Guid gameId, Guid? characterId);
    }
}