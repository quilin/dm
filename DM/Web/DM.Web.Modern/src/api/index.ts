import axios, {
  AxiosRequestConfig,
  AxiosInstance,
  AxiosResponse,
} from 'axios';
import { ApiResult } from '@/api/models/common';
import { BbRenderMode } from './bbRenderMode';

const tokenKey = 'x-dm-auth-token';
const renderKey = 'x-dm-bb-render-mode';

const defaultHeaders: { [key: string]: string } = {
  'Cache-Control': 'no-cache',
  'Content-Type': 'application/json',
  [renderKey]: 'html',
};

const storedToken = localStorage.getItem(tokenKey);
if (storedToken) {
  defaultHeaders[tokenKey] = storedToken!;
}

const configuration: AxiosRequestConfig = {
  baseURL: `${process.env.API_URL ?? 'http://localhost:5000'}/v1`,
  headers: defaultHeaders,
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

  public async post<T>(url: string, params?: any): Promise<ApiResult<T>> {
    return this.send(() => this.axios.post(url, params));
  }

  /*
   Поскольку мы отслеживаем прогресс только отправки файла на сервер с клиента,
   обработчик прогресса "зависает" на 99% до момента получения окончательного ответа от сервера.
  */
  public async postFile<T>(url: string, params?: any, progressCallback?: (event: ProgressEvent) => void): Promise<ApiResult<T>> {
    const result = await this.send<T>(() => this.axios.post(url, params, {
      onUploadProgress: progressCallback
        ? (event: ProgressEvent) =>
          progressCallback(event.loaded === event.total ? { loaded: 99, total: 100 } as ProgressEvent : event)
        : undefined,
    }));
    progressCallback?.({ loaded: 1, total: 1 } as ProgressEvent);
    return result;
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
      }
      return {
        data: data as T,
        error: null,
      };
    } catch (err) {
      return {
        data: null,
        error: err.response ? {...err.response.data.error, code: err.response.status} : err,
      };
    }
  }

  public restoreAuthentication() {
    const token = localStorage.getItem(tokenKey);
    if (token === null) return;

    this.axios.defaults.headers.common[tokenKey] = token;
    localStorage.setItem(tokenKey, token);
  }

  public logout() {
    delete this.axios.defaults.headers.common[tokenKey];
    localStorage.removeItem(tokenKey);
  }
}

export default new Api();
