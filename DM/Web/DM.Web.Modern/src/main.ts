import Vue from 'vue';
import VModal from 'vue-js-modal';
import Vuelidate from 'vuelidate';
import App from './App.vue';

import Icon from '@/components/Icon.vue';

import Lightbox from '@/components/Lightbox.vue';

import Loader from '@/components/Loader.vue';
import HumanTimespan from '@/components/HumanTimespan.vue';
import Paging from '@/components/Paging.vue';

import TextArea from '@/components/inputs/TextArea.vue';
import ActionButton from '@/components/inputs/ActionButton.vue';
import Dropdown from '@/components/inputs/Dropdown.vue';
import SuggestInput from '@/components/inputs/SuggestInput.vue';

import Rating from '@/components/community/Rating.vue';
import Online from '@/components/community/Online.vue';

import moment from 'moment';

import router from './router';
import store from './store';

Vue.use(VModal);
Vue.use(Vuelidate);

Vue.config.productionTip = false;
Vue.component('icon', Icon);
Vue.component('lightbox', Lightbox);
Vue.component('loader', Loader);
Vue.component('humanTimespan', HumanTimespan);
Vue.component('paging', Paging);

Vue.component('textArea', TextArea);
Vue.component('actionButton', ActionButton);
Vue.component('dropdown', Dropdown);
Vue.component('suggestInput', SuggestInput);

Vue.component('rating', Rating);
Vue.component('online', Online);

moment.locale('ru');

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
