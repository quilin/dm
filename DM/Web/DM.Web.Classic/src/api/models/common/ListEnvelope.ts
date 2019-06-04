import Paging from '@/api/models/common/Paging';

export default class ListEnvelope<T> {
  public resources: T[];
  public paging: Paging;
}
