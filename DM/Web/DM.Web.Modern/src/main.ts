import Vue from 'vue';
import VModal from 'vue-js-modal';
import Vuelidate from 'vuelidate';
import PortalVue from 'portal-vue';
import App from './App.vue';

import Icon from '@/components/Icon.vue';

import Lightbox from '@/components/Lightbox.vue';

import HumanTimespan from '@/components/dates/HumanTimespan.vue';
import HumanDate from '@/components/dates/HumanDate.vue';

import Loader from '@/components/Loader.vue';
import Paging from '@/components/Paging.vue';

import TextArea from '@/components/inputs/TextArea.vue';
import ActionButton from '@/components/inputs/ActionButton.vue';
import Dropdown from '@/components/inputs/Dropdown.vue';
import SuggestInput from '@/components/inputs/SuggestInput.vue';
import Upload from '@/components/inputs/Upload.vue';

import Rating from '@/components/community/Rating.vue';
import Online from '@/components/community/Online.vue';
import UserLink from '@/components/community/UserLink.vue';

import moment from 'moment';

import router from './router';
import store from './store';

Vue.use(VModal);
Vue.use(Vuelidate);
Vue.use(PortalVue);

Vue.config.productionTip = false;
Vue.component('icon', Icon);
Vue.component('lightbox', Lightbox);

Vue.component('humanTimespan', HumanTimespan);
Vue.component('humanDate', HumanDate);

Vue.component('loader', Loader);
Vue.component('paging', Paging);

Vue.component('textArea', TextArea);
Vue.component('actionButton', ActionButton);
Vue.component('dropdown', Dropdown);
Vue.component('suggestInput', SuggestInput);
Vue.component('upload', Upload);

Vue.component('rating', Rating);
Vue.component('online', Online);
Vue.component('userLink', UserLink);

moment.locale('ru');

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
