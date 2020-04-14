import * as types from './../mutation-types'

export default {
  state: {
    interfaceSidebarShow: false,
  },
  getters: {
    getInterfaceSidebarShow: state => state.interfaceSidebarShow,
  },
  actions: {
    setInterfaceSidebarShow({commit}, displaySidebar) {
      commit(types.INTERFACE_SIDEBAR_SHOW_SET, displaySidebar);
    }
  },
  mutations: {
    [types.INTERFACE_SIDEBAR_SHOW_SET]: (state, status) => state.interfaceSidebarShow = status,
  },
}
