using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.GameNotepad
{
    public interface IGameNotepadFormBuilder
    {
        Task<GameNotepadForm> Build(Guid gameId);
    }
}