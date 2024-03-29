﻿using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.Shared.Commentaries
{
    public interface ICommentariesViewModelBuilder
    {
        Task<CommentariesViewModel> Build(Guid entityId, int? entityNumber, bool canComment);
        Task<CommentaryViewModel[]> BuildList(Guid entityId, int? entityNumber);
    }
}