using AutoMapper;

namespace DM.Web.API.BbRendering;

/// <inheritdoc />
internal class BbTextProfile : Profile
{
    /// <inheritdoc />
    public BbTextProfile()
    {
        CreateMap<BbText, string>()
            .IncludeAllDerived()
            .ConvertUsing(v => v == null ? null : v.Value);

        CreateMap<string, ChatBbText>().ConvertUsing(v => new ChatBbText {Value = v});
        CreateMap<string, PostBbText>().ConvertUsing(v => new PostBbText {Value = v});
        CreateMap<string, CommonBbText>().ConvertUsing(v => new CommonBbText {Value = v});
        CreateMap<string, InfoBbText>().ConvertUsing(v => new InfoBbText {Value = v});
    }
}