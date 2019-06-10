import axios, {
  AxiosRequestConfig,
  AxiosInstance,
  AxiosResponse,
  Canceler,
} from 'axios';
import GeneralError from '@/api/models/common';

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

  public async get<T>(url: string, params?: any): Promise<T | GeneralError> {
    return this.send(() => this.axios.get(url, { params }));
  }

  public async post<T>(url: string, params: any): Promise<T | GeneralError> {
    return this.send(() => this.axios.post(url, params));
  }

  public async put<T>(url: string, params: any): Promise<T | GeneralError> {
    return this.send(() => this.axios.put(url, params));
  }

  public async delete(url: string): Promise<void | GeneralError> {
    await this.send(() => this.axios.delete(url));
  }

  private async send<T>(sender: () => Promise<AxiosResponse<T>>): Promise<T | GeneralError> {
    try {
      const { data, headers } = await sender();
      if ((tokenKey in headers)) {
        const token = headers[tokenKey];
        this.axios.defaults.headers.common[tokenKey] = token;
        localStorage.setItem(tokenKey, token);
      } else {
        delete this.axios.defaults.headers.common[tokenKey];
        localStorage.removeItem(tokenKey);
      }
      return data as T;
    } catch (err) {
      return err.response.data.error;
    }
  }
}

export default new Api();
