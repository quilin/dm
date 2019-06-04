import Paging from '@/api/models/common/Paging';

export default interface ListEnvelope<T> {
  resources: T[];
  paging: Paging;
}
