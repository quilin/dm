using System.Collections.Generic;
using BBCodeParser.Tags;

namespace Web.Core.Parsers
{
    public class TagSet
    {
        private readonly List<Tag> set;

        public TagSet(Tag[] defaultSet)
        {
            set = new List<Tag>(defaultSet);
        }

        public Tag[] ToArray()
        {
            return set.ToArray();
        }

        public TagSet With(params Tag[] tags)
        {
            set.AddRange(tags);
            return this;
        }

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