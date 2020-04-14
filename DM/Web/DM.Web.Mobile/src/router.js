import Vue from 'vue'
import VueRouter from 'vue-router'

import FooterDefault from "./components/layout/FooterDefault";

Vue.use(VueRouter)

export default new VueRouter({
  routes: [
    {
      path: '/',
      name: 'home',
      components: {
        footer: FooterDefault,
      }
    },

  ],
  mode: 'history'
})
