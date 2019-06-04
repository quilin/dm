import Vue from 'vue';
import App from './App.vue';
import Icon from '@/components/Icon.vue';
import router from './router';
import store from './store';

Vue.config.productionTip = false;
Vue.component('icon', Icon);

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
