export const LS_AUTH_KEY = '__Auth_Token__'
export const LS_USER_KEY = '__Current_User__'

/* eslint-env browser */
const ls = localStorage

export default {
  set: (key, value) => ls.setItem(key, JSON.stringify(value)),
  get: key => JSON.parse(ls.getItem(key)),
  drop: key => ls.removeItem(key)
}
