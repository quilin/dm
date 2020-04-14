import axios from 'axios'
import qs from 'qs'
import { baseUrl } from './api-constants'
import * as ls from './ls'

const authHeader = 'X-Dm-Auth-Token'

let authToken = ls.default.get(ls.LS_AUTH_KEY)
let headers = {
  'Content-Type': 'application/json; charset=utf-8'
}
if (authToken) {
  headers[authHeader] = authToken
}

const axiosInstance = axios.create({
  baseURL: baseUrl,
  headers: headers,
  responseType: 'json'
})

export const saveAuth = (data) => {
  if (!ls || !data.token) return
  axiosInstance.defaults.headers.common[authHeader] = data.token
  ls.default.set(ls.LS_AUTH_KEY, data.token)
  ls.default.set(ls.LS_USER_KEY, data.user)
}

export const dropAuth = () => {
  if (!ls) return
  axiosInstance.defaults.headers.common[authHeader] = ''
  ls.default.drop(ls.LS_AUTH_KEY)
  ls.default.drop(ls.LS_USER_KEY)
}

export const get = (url, data) => {
  let dataEncoded = qs.stringify(data, {
    indices: false
  })
  return axiosInstance.get(`${url}${data ? '/?' : ''}${dataEncoded}`)
}

export const post = (url, data) => axiosInstance.post(url, data)
