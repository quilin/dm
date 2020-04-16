import Vue from 'vue'
import App from './App.vue'

import moment from 'moment';

import Icon from '@/components/Icon.vue';
import Loader from '@/components/Loader.vue';
import HumanTimespan from '@/components/HumanTimespan.vue';
import Header from "@/components/Header.vue";

import router from './router'
import store from './store'

Vue.component('icon', Icon);
Vue.component('loader', Loader);
Vue.component('humanTimespan', HumanTimespan);
Vue.component('Header', Header);

moment.locale('ru');

Vue.config.productionTip = false

new Vue({
    router,
    store,
    render: h => h(App)
}).$mount('#app')
