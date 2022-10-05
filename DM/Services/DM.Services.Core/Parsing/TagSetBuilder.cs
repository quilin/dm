using System.Collections.Generic;
using BBCodeParser.Tags;

namespace DM.Services.Core.Parsing;

/// <summary>
/// BBCode parser tag set
/// </summary>
internal class TagSetBuilder
{
    private readonly List<Tag> set;

    /// <inheritdoc />
    public TagSetBuilder(IEnumerable<Tag> defaultSet)
    {
        set = new List<Tag>(defaultSet);
    }

    /// <summary>
    /// Build set of tags
    /// </summary>
    /// <returns></returns>
    public Tag[] Build()
    {
        return set.ToArray();
    }

    /// <summary>
    /// Add tags to set
    /// </summary>
    /// <param name="tags">Tags</param>
    /// <returns>Self</returns>
    public TagSetBuilder With(params Tag[] tags)
    {
        set.AddRange(tags);
        return this;
    }

    /// <summary>
    /// Remove tags from set
    /// </summary>
    /// <param name="tags">Tags</param>
    /// <returns>Self</returns>
    public TagSetBuilder Without(params Tag[] tags)
    {
        foreach (var tag in tags)
        {
            set.Remove(tag);
        }
        return this;
    }
}