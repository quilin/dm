import type { Envelope } from "@/api/models/common";
import type { User } from "@/api/models/community";
import type {
  LoginCredentials,
  RegisterCredentials,
} from "@/api/models/account";
import Api from "@/api";

export default new (class AccountApi {
  public register(credentials: RegisterCredentials) {
    return Api.post<Envelope<User>>("account", credentials);
  }
  public activate(token: string) {
    return Api.put<Envelope<User>>(`account/${token}`);
  }

  public signIn(credentials: LoginCredentials) {
    return Api.post<Envelope<User>>("account/login", credentials);
  }
  public async fetchUser() {
    return await Api.get<Envelope<User>>("account");
  }
  public async signOut() {
    const result = await Api.delete("account/login");
    Api.logout();
    return result;
  }
  public isAuthenticated(): boolean {
    return Api.isAuthenticated();
  }
})();
