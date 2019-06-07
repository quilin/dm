import { Envelope, BadRequestError } from '@/api/models/common';
import { LoginCredentials, User } from '@/api/models/community';
import Api from '@/api';

export default new class AccountApi {
  public async signIn(credentials: LoginCredentials): Promise<Envelope<User> | BadRequestError> {
    return await Api.post('account/login', credentials);
  }
  public async fetchUser(): Promise<Envelope<User>> {
    return await Api.get('account');
  }
  public async signOut(): Promise<void> {
    await Api.delete('account/login');
  }
}();
