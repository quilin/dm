import Vue from 'vue'
import Vuex from 'vuex'

import Interface from './Interface'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    Interface,
  }
})
