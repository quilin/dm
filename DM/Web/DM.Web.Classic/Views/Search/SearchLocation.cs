using System.ComponentModel;

namespace DM.Web.Classic.Views.Search
{
    public enum SearchLocation
    {
        [Description("везде")] Everywhere = 0,
        [Description("на форуме")] Forum = 1,
        [Description("в играх")] Games = 2,
        [Description("в сообществе")] Community = 3
    }
}