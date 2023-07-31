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

import PageTitle from "@/components/layout/PageTitle.vue";
import BlockTitle from "@/components/layout/BlockTitle.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import TheLoader from "@/components/TheLoader.vue";
import HumanDate from "@/components/dates/HumanDate.vue";
import HumanTimespan from "@/components/dates/HumanTimespan.vue";
import TheIcon from "@/components/icons/TheIcon.vue";
import TheLightbox from "@/components/layout/TheLightbox.vue";
import TheButton from "@/components/inputs/TheButton.vue";
import TheForm from "@/components/inputs/form/TheForm.vue";
import FormField from "@/components/inputs/form/FormField.vue";
import { createI18n } from "vue-i18n";
import UserLink from "@/components/community/UserLink.vue";

dayjs.extend(relativeTime).locale("ru");

const i18n = createI18n({
  locale: "ru",
});

const application = createApp(App);

application.config.globalProperties.IconType = IconType;

application
  .component("TheLoader", TheLoader)
  .component("TheIcon", TheIcon)
  .component("PageTitle", PageTitle)
  .component("BlockTitle", BlockTitle)
  .component("SecondaryText", SecondaryText)
  .component("TheForm", TheForm)
  .component("FormField", FormField)
  .component("TheButton", TheButton)
  .component("TheLightbox", TheLightbox)
  .component("HumanDate", HumanDate)
  .component("HumanTimespan", HumanTimespan)
  .component("UserLink", UserLink);

application
  .use(createPinia())
  .use(router)
  .use(createVfm())
  .use(i18n)
  .mount("#application");
