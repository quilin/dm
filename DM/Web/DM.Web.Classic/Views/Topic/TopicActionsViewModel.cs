using System;
using System.Collections.Generic;

namespace DM.Web.Classic.Views.Topic
{
    public class TopicActionsViewModel
    {
        public Guid TopicId { get; set; }
        public string TopicTitle { get; set; }

        public bool CanComment { get; set; }
        public bool CanClose { get; set; }
        public bool CanOpen { get; set; }
        public bool CanAttach { get; set; }
        public bool CanDetach { get; set; }
        public bool CanEdit { get; set; }
        public bool CanRemove { get; set; }
        
        public bool CanMove { get; set; }
        public IDictionary<string, object> Forums { get; set; }

        public bool HasActions => CanAttach || CanDetach || CanClose || CanOpen || CanRemove || CanEdit || CanMove;
    }
}