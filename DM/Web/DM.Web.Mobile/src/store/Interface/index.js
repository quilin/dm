import * as types from './../mutation-types'

export default {
  state: {
    interfaceSidebarShow: false,
  },
  getters: {
    getInterfaceSidebarShow: state => state.interfaceSidebarShow,
  },
  actions: {
    async setInterfaceSidebarShow({commit}, {status}) {
      commit(types.INTERFACE_SIDEBAR_SHOW_SET, {status});
    }
  },
  mutations: {
    [types.INTERFACE_SIDEBAR_SHOW_SET]: (state, status) => state.interfaceSidebarShow = status,
  },
}
