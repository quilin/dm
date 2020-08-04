import axios, {
  AxiosRequestConfig,
  AxiosInstance,
  AxiosResponse,
} from 'axios';
import { ApiResult } from '@/api/models/common';
import { BbRenderMode } from './bbRenderMode';

const tokenKey = 'x-dm-auth-token';
const renderKey = 'x-dm-bb-render-mode';

const defautlHeaders: { [key: string]: string } = {
  'Cache-Control': 'no-cache',
  'Content-Type': 'application/json',
  [renderKey]: 'html',
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

  public async get<T>(
    url: string,
    params?: any,
    bbRenderMode: BbRenderMode = BbRenderMode.Html): Promise<ApiResult<T>> {
    return this.send(() => this.axios.get(url, { params, headers: {[renderKey]: bbRenderMode} }));
  }

  public async post<T>(url: string, params: any): Promise<ApiResult<T>> {
    return this.send(() => this.axios.post(url, params));
  }

  public async put<T>(url: string, params?: any): Promise<ApiResult<T>> {
    return this.send(() => this.axios.put(url, params));
  }

  public async patch<T>(url: string, params: any): Promise<ApiResult<T>> {
    return this.send(() => this.axios.patch(url, params));
  }

  public async delete<T>(url: string): Promise<ApiResult<T>> {
    return await this.send(() => this.axios.delete(url));
  }

  private async send<T>(sender: () => Promise<AxiosResponse<T>>): Promise<ApiResult<T>> {
    try {
      const { data, headers } = await sender();
      if (tokenKey in headers) {
        const token = headers[tokenKey];
        this.axios.defaults.headers.common[tokenKey] = token;
        localStorage.setItem(tokenKey, token);
      } else {
        delete this.axios.defaults.headers.common[tokenKey];
        localStorage.removeItem(tokenKey);
      }
      return {
        data: data as T,
        error: null,
      };
    } catch (err) {
      return {
        data: null,
        error: {...err.response.data.error, code: err.response.status},
      };
    }
  }
}

export default new Api();
