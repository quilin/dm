import dayjs from "dayjs";
import relativeTime from "dayjs/plugin/relativeTime";
import "dayjs/locale/ru";

import { createApp } from "vue";
import { createPinia } from "pinia";
import { createVfm } from "vue-final-modal";

import App from "./App.vue";
import router from "./router";

import "vue-final-modal/style.css";
import "@/assets/styles/Reset.sass";
import "@/assets/styles/Fonts.sass";
import "@/assets/styles/Inputs.sass";

import { IconType } from "@/components/icons/iconType";

import SecondaryText from "@/components/layout/SecondaryText.vue";
import TheLoader from "@/components/TheLoader.vue";
import HumanDate from "@/components/dates/HumanDate.vue";
import HumanTimespan from "@/components/dates/HumanTimespan.vue";
import TheIcon from "@/components/icons/TheIcon.vue";
import TheLightbox from "@/components/layout/TheLightbox.vue";
import TheButton from "@/components/inputs/TheButton.vue";

dayjs.extend(relativeTime);

const application = createApp(App);

application.config.globalProperties.IconType = IconType;

application
  .component("TheIcon", TheIcon)
  .component("SecondaryText", SecondaryText)
  .component("TheButton", TheButton)
  .component("TheLoader", TheLoader)
  .component("TheLightbox", TheLightbox)
  .component("HumanDate", HumanDate)
  .component("HumanTimespan", HumanTimespan);

application
  .use(createPinia())
  .use(router)
  .use(createVfm())
  .mount("#application");
