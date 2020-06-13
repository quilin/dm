import Vue from 'vue'
import Vuelidate from 'vuelidate';
import App from './App.vue'

import moment from 'moment';

import Icon from '@/components/Icon.vue';
import Loader from '@/components/Loader.vue';
import HumanTimespan from '@/components/HumanTimespan.vue';
import Vue2TouchEvents from 'vue2-touch-events'

import router from './router'
import store from './store'

Vue.use(Vuelidate);
Vue.use(Vue2TouchEvents);

Vue.component('icon', Icon);
Vue.component('loader', Loader);
Vue.component('humanTimespan', HumanTimespan);

moment.locale('ru');

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
