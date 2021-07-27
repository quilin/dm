import { Envelope, ApiResult } from '@/api/models/common';
import { LoginCredentials, User } from '@/api/models/community';
import Api from '@/api';

export default new class AccountApi {
  public async signIn(credentials: LoginCredentials): Promise<ApiResult<Envelope<User>>> {
    return await Api.post('account/login', credentials);
  }
  public async fetchUser(): Promise<ApiResult<Envelope<User>>> {
    return await Api.get('account');
  }
  public async signOut(): Promise<ApiResult<void>> {
    Api.logout();
    return await Api.delete('account/login');
  }
  public restoreUser(): void {
    Api.restoreAuthentication();
  }
}();
