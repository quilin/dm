using System;

namespace DM.Web.Classic.Views.Shared.Commentaries
{
    public interface ICreateCommentaryFormBuilder
    {
        CreateCommentaryForm Build(Guid entityId);
    }
}