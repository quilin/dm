using System.ComponentModel.DataAnnotations;

namespace DM.Web.Classic.Views.Search
{
    public class SearchForm
    {
        [Required(ErrorMessage = "Введите что-нибудь")]
        public string Query { get; set; }

        public SearchLocation Location { get; set; }
    }
}