using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Games.Updating;

/// <summary>
/// Service for game update
/// </summary>
public interface IGameUpdatingService
{
    /// <summary>
    /// Update existing game
    /// </summary>
    /// <param name="updateGame">Update game model</param>
    /// <returns></returns>
    Task<GameExtended> Update(UpdateGame updateGame);
}