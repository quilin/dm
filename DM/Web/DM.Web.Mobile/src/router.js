import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

export default new VueRouter({
  routes: [
    {
      path: '/',
      name: 'home',
      components: {
        footer: () => import('@/components/layout/footer.vue'),
      }
    },

  ],
  mode: 'history'
})
