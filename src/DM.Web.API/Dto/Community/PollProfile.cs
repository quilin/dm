using System.Linq;
using AutoMapper;
using DM.Services.Community.BusinessProcesses.Polls.Creating;
using DtoPoll = DM.Services.Community.BusinessProcesses.Polls.Reading.Poll;
using DtoPollOption = DM.Services.Community.BusinessProcesses.Polls.Reading.PollOption;

namespace DM.Web.API.Dto.Community;

/// <inheritdoc />
internal class PollProfile : Profile
{
    /// <inheritdoc />
    public PollProfile()
    {
        CreateMap<DtoPoll, Poll>()
            .ForMember(d => d.Ends, s => s.MapFrom(p => p.EndDate));
        CreateMap<DtoPollOption, PollOption>()
            .ForMember(d => d.VotesCount, s => s.MapFrom(o => o.UserIds.Count()))
            .ForMember(d => d.Voted, s => s.MapFrom<PollParticipationResolver>());

        CreateMap<Poll, CreatePoll>()
            .ForMember(d => d.Title, s => s.MapFrom(p => p.Title))
            .ForMember(d => d.EndDate, s => s.MapFrom(p => p.Ends))
            .ForMember(d => d.Options, s => s.MapFrom(p => p.Options.Select(o => o.Text)));
    }
}