import Paging from '@/api/models/common/paging';

export default interface ListEnvelope<T> {
  resources: T[];
  paging: Paging;
}
