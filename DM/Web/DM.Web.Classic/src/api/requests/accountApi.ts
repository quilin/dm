import LoginCredentials from '@/api/models/community/loginCredentials';
import Envelope from '@/api/models/common/envelope';
import User from '@/api/models/community/user';
import Api from '@/api';

export default class AccountApi {
  public async login(credentials: LoginCredentials): Promise<Envelope<User>> {
    return await Api.post('account/login', credentials);
  }
}
