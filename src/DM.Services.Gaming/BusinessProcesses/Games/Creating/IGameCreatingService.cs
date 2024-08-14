using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Games.Creating;

/// <summary>
/// Service to create games
/// </summary>
public interface IGameCreatingService
{
    /// <summary>
    /// Create new game
    /// </summary>
    /// <returns></returns>
    Task<GameExtended> Create(CreateGame createGame);
}