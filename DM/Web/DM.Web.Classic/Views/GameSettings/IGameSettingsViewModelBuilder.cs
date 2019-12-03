using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.GameSettings
{
    public interface IGameSettingsViewModelBuilder
    {
        Task<GameSettingsViewModel> Build(Guid gameId, GameSettingsType settingsType);
    }
}