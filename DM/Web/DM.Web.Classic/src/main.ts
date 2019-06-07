import Vue from 'vue';
import VModal from 'vue-js-modal';
import App from './App.vue';

import Lightbox from '@/components/Lightbox.vue';
import Icon from '@/components/Icon.vue';
import Loader from '@/components/Loader.vue';
import HumanTimespan from '@/components/HumanTimespan.vue';

import moment from 'moment';

import router from './router';
import store from './store';

Vue.config.productionTip = false;
Vue.component('lightbox', Lightbox);
Vue.component('icon', Icon);
Vue.component('loader', Loader);
Vue.component('humanTimespan', HumanTimespan);

Vue.use(VModal);

moment.locale('ru');

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
