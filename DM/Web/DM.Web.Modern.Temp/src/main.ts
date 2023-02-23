import dayjs from "dayjs";
import relativeTime from "dayjs/plugin/relativeTime";
import "dayjs/locale/ru";

import { createApp } from "vue";
import { createPinia } from "pinia";
import { createVfm } from "vue-final-modal";
import Popper from "vue3-popper";

import App from "./App.vue";
import router from "./router";

import "@/assets/styles/Reset.sass";
import "@/assets/styles/Fonts.sass";
import "vue-final-modal/style.css";

import { iconPlugin } from "@/components/icons/iconPlugin";
import { IconType } from "@/components/icons/iconType";

import SecondaryText from "@/components/layout/SecondaryText.vue";
import TheLoader from "@/components/TheLoader.vue";
import HumanDate from "@/components/dates/HumanDate.vue";
import HumanTimespan from "@/components/dates/HumanTimespan.vue";
import TheIcon from "@/components/icons/TheIcon.vue";

dayjs.extend(relativeTime);

const application = createApp(App);

application.config.globalProperties.IconType = IconType;

application
  .component("TheIcon", TheIcon)
  .component("SecondaryText", SecondaryText)
  .component("TheLoader", TheLoader)
  .component("HumanDate", HumanDate)
  .component("HumanTimespan", HumanTimespan)
  .component("ThePopper", Popper);

application
  .use(iconPlugin)
  .use(createPinia())
  .use(router)
  .use(createVfm())
  .mount("#application");
