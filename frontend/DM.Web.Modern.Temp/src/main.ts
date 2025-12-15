import dayjs from "dayjs";
import relativeTime from "dayjs/plugin/relativeTime";
import "dayjs/locale/ru";

import { createApp } from "vue";
import { createPinia } from "pinia";
import { createVfm } from "vue-final-modal";

import App from "./App.vue";
import router from "./router";
import i18n from "./i18n";

import PrimeVue from "primevue/config";

import "vue-final-modal/style.css";
import "@/assets/styles/Reset.sass";
import "@/assets/styles/Fonts.sass";
import "@/assets/styles/Inputs.sass";

dayjs.extend(relativeTime).locale("en");

const application = createApp(App);

application
  .use(createPinia())
  .use(router)
  .use(PrimeVue, {
    unstyled: true,
  })
  .use(createVfm())
  .use(i18n)
  .mount("#application");
