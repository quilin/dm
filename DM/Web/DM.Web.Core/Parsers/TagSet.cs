using System.Collections.Generic;
using BBCodeParser.Tags;

namespace DM.Web.Core.Parsers
{
    /// <summary>
    /// BBCode parser tag set
    /// </summary>
    public class TagSet
    {
        private readonly List<Tag> set;

        /// <inheritdoc />
        public TagSet(Tag[] defaultSet)
        {
            set = new List<Tag>(defaultSet);
        }

        /// <summary>
        /// Get tags
        /// </summary>
        /// <returns></returns>
        public Tag[] ToArray()
        {
            return set.ToArray();
        }

        /// <summary>
        /// Add tags to set
        /// </summary>
        /// <param name="tags">Tags</param>
        /// <returns>Self</returns>
        public TagSet With(params Tag[] tags)
        {
            set.AddRange(tags);
            return this;
        }

        /// <summary>
        /// Remove tags from set
        /// </summary>
        /// <param name="tags">Tags</param>
        /// <returns>Self</returns>
        public TagSet Without(params Tag[] tags)
        {
            foreach (var tag in tags)
            {
                set.Remove(tag);
            }
            return this;
        }
    }
}