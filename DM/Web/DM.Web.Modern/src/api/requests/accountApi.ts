import { Envelope, ApiResult } from '@/api/models/common';
import { LoginCredentials, RegisterCredentials, User } from '@/api/models/community';
import Api from '@/api';

export default new class AccountApi {
  public async register(credentials: RegisterCredentials): Promise<ApiResult<Envelope<User>>> {
    return await Api.post('account', credentials);
  }
  public async activate(token: string): Promise<ApiResult<Envelope<User>>> {
    return await Api.put(`account/${token}`);
  }

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
