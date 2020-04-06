using System.ComponentModel.DataAnnotations;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.Classic.Views.Search
{
    public class SearchForm
    {
        [Required(ErrorMessage = "Введите что-нибудь")]
        public string Query { get; set; }

        public SearchEntityType SearchEntityType { get; set; }
    }
}