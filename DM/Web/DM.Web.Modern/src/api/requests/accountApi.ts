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
    const result = await Api.delete<void>('account/login');
    Api.logout();
    return result;
  }
  public isAuthenticated(): boolean {
    return Api.isAuthenticated();
  }
}();
