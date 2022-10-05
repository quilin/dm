using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.GeneralBus;

namespace DM.Services.Search.Consumer.Implementation.Indexing.Indexers;

/// <inheritdoc />
internal class CommentDeletedIndexer : BaseIndexer
{
    private readonly IIndexingRepository indexingRepository;

    /// <inheritdoc />
    public CommentDeletedIndexer(
        IIndexingRepository indexingRepository)
    {
        this.indexingRepository = indexingRepository;
    }
    
    /// <inheritdoc />
    protected override EventType EventType => EventType.DeletedForumComment;

    /// <inheritdoc />
    public override Task Index(InvokedEvent message)
    {
        return indexingRepository.Delete(message.EntityId);
    }
}