using System.Collections.Generic;

namespace DM.Web.API.Dto.Contracts
{
    public class Envelope<T>
    {
        public T Resource { get; set; }
    }

    public class ListEnvelope<T>
    {
        public IEnumerable<T> Resources { get; set; }
        public Paging Paging { get; set; }
    }
}