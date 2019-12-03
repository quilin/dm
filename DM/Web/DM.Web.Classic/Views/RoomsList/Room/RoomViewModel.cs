using System;
using System.ComponentModel.DataAnnotations;
using DM.Services.Core.Dto.Enums;
using DM.Web.Classic.Views.RoomsList.Character;
using DM.Web.Classic.Views.RoomsList.Room.RoomActions;

namespace DM.Web.Classic.Views.RoomsList.Room
{
    public class RoomViewModel
    {
        public Guid RoomId { get; set; }

        [Required(ErrorMessage = "Введите название комнаты")]
        [StringLength(30, ErrorMessage = "Название не должно быть длиннее 30 символов")]
        public string Title { get; set; }
        public RoomType RoomType { get; set; }
        public RoomAccessType AccessType { get; set; }
        public RoomActionsViewModel RoomActions { get; set; }
        public CharacterViewModel[] Characters { get; set; }
    }
}