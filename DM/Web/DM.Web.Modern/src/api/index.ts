import axios, {
  AxiosRequestConfig,
  AxiosInstance,
  AxiosResponse,
} from 'axios';
import qs from 'qs';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ApiResult } from '@/api/models/common';
import { BbRenderMode } from './bbRenderMode';
import Config from '@/config.json';

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

const apiHost = Config.API_URL ?? process.env.API_URL;

const configuration: AxiosRequestConfig = {
  baseURL: `${apiHost}/v1`,
  headers: defaultHeaders,
  responseType: 'json',
  paramsSerializer: params => qs.stringify(params),
};

class Api {
  private axios: AxiosInstance;
  private authToken: string | null = null;

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
        this.authToken = token;
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
    this.authToken = token;
  }

  public logout() {
    delete this.axios.defaults.headers.common[tokenKey];
    localStorage.removeItem(tokenKey);
    this.authToken = null;
  }

  public establishHubConnection(path: string): HubConnection {
    const token = this.authToken;
    return new HubConnectionBuilder()
      .withUrl(`${apiHost}/${path}`, {
        accessTokenFactory() {
          return token!;
        }
      })
      .withAutomaticReconnect()
      .build();
  }
}

export default new Api();
