import LoginCredentials from '@/api/models/community/LoginCredentials';
import Envelope from '@/api/models/common/Envelope';
import User from '@/api/models/community/User';

export default class Account {
  public login(credentials: LoginCredentials): Envelope<User> {
    
  }
}