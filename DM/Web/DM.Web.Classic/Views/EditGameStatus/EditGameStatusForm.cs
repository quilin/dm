using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.Classic.Views.EditGameStatus
{
    public class EditGameStatusForm
    {
        public Guid ModuleId { get; set; }
        public GameStatus Status { get; set; }
    }
}