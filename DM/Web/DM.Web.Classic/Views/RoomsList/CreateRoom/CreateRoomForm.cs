using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Web.Classic.Views.RoomsList.CreateRoom
{
    public class CreateRoomForm
    {
        public Guid GameId { get; set; }

        [Required(ErrorMessage = "Введите название комнаты")]
        [StringLength(100, ErrorMessage = "Название не может содержать больше {1} символов")]
        public string Title { get; set; }
    }
}