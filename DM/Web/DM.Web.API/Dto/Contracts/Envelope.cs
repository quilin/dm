using System.Collections.Generic;

namespace DM.Web.API.Dto.Contracts
{
    public class Envelope<T>
    {
        public Envelope(T resource)
        {
            Resource = resource;
        }
        
        public T Resource { get; }
    }

    public class ListEnvelope<T>
    {
        public ListEnvelope(IEnumerable<T> resources, Paging paging = null)
        {
            Resources = resources;
            Paging = paging;
        }
        
        public IEnumerable<T> Resources { get; }
        public Paging Paging { get; }
    }
}