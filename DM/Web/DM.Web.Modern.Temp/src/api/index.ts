import axios from "axios";
import qs from "qs";

import type {
  AxiosRequestConfig,
  AxiosInstance,
  AxiosResponse,
  AxiosProgressEvent,
} from "axios";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import type { ApiResult } from "@/api/models/common";
import { BbRenderMode } from "./bbRenderMode";

const tokenKey = "x-dm-auth-token";
const renderKey = "x-dm-bb-render-mode";

const defaultHeaders: { [key: string]: string } = {
  "Cache-Control": "no-cache",
  "Content-Type": "application/json",
  [renderKey]: "html",
};

const apiHost = "http://localhost:5050"; // Config

const configuration: AxiosRequestConfig = {
  baseURL: `${apiHost}/v1`,
  headers: defaultHeaders,
  responseType: "json",
  paramsSerializer: {
    encode: (params) => qs.stringify(params),
  },
};

class Api {
  private axios: AxiosInstance;

  constructor() {
    this.axios = axios.create(configuration);

    const token = localStorage.getItem(tokenKey);
    if (token) {
      this.updateAuthenticationInfo(token);
    }
  }

  public isAuthenticated(): boolean {
    return tokenKey in this.axios.defaults.headers.common;
  }

  public async get<T>(
    url: string,
    params?: any,
    bbRenderMode: BbRenderMode = BbRenderMode.Html
  ): Promise<ApiResult<T>> {
    return this.send(() =>
      this.axios.get(url, { params, headers: { [renderKey]: bbRenderMode } })
    );
  }

  public async post<T>(url: string, params?: any): Promise<ApiResult<T>> {
    return this.send(() => this.axios.post(url, params));
  }

  /*
   Поскольку мы отслеживаем прогресс только отправки файла на сервер с клиента,
   обработчик прогресса "зависает" на 99% до момента получения окончательного ответа от сервера.
  */
  public async postFile<T>(
    url: string,
    params?: any,
    progressCallback?: (event: AxiosProgressEvent) => void
  ): Promise<ApiResult<T>> {
    const result = await this.send<T>(() =>
      this.axios.post(url, params, {
        onUploadProgress: progressCallback
          ? (event: AxiosProgressEvent) =>
              progressCallback(
                event.loaded === event.total
                  ? ({ loaded: 99, total: 100 } as AxiosProgressEvent)
                  : event
              )
          : undefined,
      })
    );
    progressCallback?.({ loaded: 1, total: 1 } as AxiosProgressEvent);
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

  private async send<T>(
    sender: () => Promise<AxiosResponse<T>>
  ): Promise<ApiResult<T>> {
    try {
      const { data, headers } = await sender();
      if (tokenKey in headers) {
        this.updateAuthenticationInfo(headers[tokenKey]);
      }
      return {
        data: data as T,
        error: null,
      };
    } catch (err: any) {
      return {
        data: null,
        error: err.response
          ? { ...err.response.data.error, code: err.response.status }
          : err,
      };
    }
  }

  public logout() {
    this.clearAuthenticationInfo();
  }

  public establishHubConnection(path: string): HubConnection {
    const token = this.axios.defaults.headers.common[tokenKey] as string;
    return new HubConnectionBuilder()
      .withAutomaticReconnect()
      .withUrl(`${apiHost}/${path}`, {
        accessTokenFactory() {
          return token!;
        },
      })
      .build();
  }

  private updateAuthenticationInfo(token: string): void {
    this.axios.defaults.headers.common[tokenKey] = token;
    localStorage.setItem(tokenKey, token);
  }

  private clearAuthenticationInfo(): void {
    delete this.axios.defaults.headers.common[tokenKey];
    localStorage.removeItem(tokenKey);
  }
}

export default new Api();
