using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Polls.Reading;

namespace DM.Web.Classic.Views.Polls
{
    public interface IPollsViewModelBuilder
    {
        Task<PollsViewModel> Build(int entityNumber);
        Task<PollsViewModel> BuildActive();
        PollViewModel Build(Poll poll);
    }
}