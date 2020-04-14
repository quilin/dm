import * as ls from './ls'
import { post, saveAuth, dropAuth } from './axios-instance'

export default {
  async authenticate (login, password) {
    let { data } = await post('account', { login, password })
    saveAuth(data)
    if (data.token) return data.user
  },

  async getCurrentUser () {
    return ls.default.get(ls.LS_USER_KEY)
  },
  async logout () {
    dropAuth()
  }

}
