using System;
using System.ComponentModel.DataAnnotations;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.Classic.Views.EditRoom
{
    public class EditRoomForm
    {
        public Guid RoomId { get; set; }

        [Required(ErrorMessage = "Введите название комнаты")]
        [StringLength(100, ErrorMessage = "Название не может содержать больше {1} символов")]
        public string RoomTitle { get; set; }
        public RoomType RoomType { get; set; }
        public RoomAccessType RoomAccess { get; set; }
    }
}