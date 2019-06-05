import axios, {
  AxiosRequestConfig,
  AxiosInstance,
  Canceler,
} from 'axios';

const configuration: AxiosRequestConfig = {
  baseURL: 'http://localhost:5000/v1',
  headers: {},
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
    const { data } = await this.axios.post(url, params);
    return data as T;
  }

  public async put<T>(url: string, params: any): Promise<T> {
    const { data } = await this.axios.put(url, params);
    return data as T;
  }

  public async delete(url: string): Promise<void> {
    await this.axios.delete(url);
  }
}

export default new Api();
