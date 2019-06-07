import axios, {
  AxiosRequestConfig,
  AxiosInstance,
  AxiosResponse,
  Canceler,
} from 'axios';

const tokenKey: string = 'x-dm-auth-token';

const defautlHeaders: { [key: string]: string } = {
  'Cache-Control': 'no-cache',
  'Content-Type': 'application/json',
};

const storedToken = localStorage.getItem(tokenKey);
if (storedToken) {
  defautlHeaders[tokenKey] = storedToken!;
}

const configuration: AxiosRequestConfig = {
  baseURL: 'http://localhost:5000/v1',
  headers: defautlHeaders,
  responseType: 'json',
};

class Api {
  private axios: AxiosInstance;

  constructor() {
    this.axios = axios.create(configuration);
  }

  public async get<T>(url: string, params?: any): Promise<T> {
    const { data } = await this.axios.get(url, params ? { params } : undefined);
    return data as T;
  }

  public async post<T>(url: string, params: any): Promise<T> {
    const { data, headers } = await this.axios.post(url, params);
    this.setAuthToken(headers[tokenKey]);
    return data as T;
  }

  public async put<T>(url: string, params: any): Promise<T> {
    const { data } = await this.axios.put(url, params);
    return data as T;
  }

  public async delete(url: string): Promise<void> {
    await this.axios.delete(url);
  }

  private setAuthToken(token: string | undefined): void {
    if (token) {
      this.axios.defaults.headers.common[tokenKey] = token;
      localStorage.setItem(tokenKey, token);
    } else {
      delete this.axios.defaults.headers.common[tokenKey];
      localStorage.removeItem(tokenKey);
    }
  }
}

export default new Api();
