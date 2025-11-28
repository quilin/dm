import type { App } from "vue";
import { IconType } from "@/components/ui-kit/iconType";
import DmIcon from "@/components/ui-kit/DmIcon.vue";
import PageTitle from "@/components/layout/PageTitle.vue";
import BlockTitle from "@/components/layout/BlockTitle.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import HumanDate from "@/components/dates/HumanDate.vue";
import HumanTimespan from "@/components/dates/HumanTimespan.vue";
import UserLink from "@/components/community/UserLink.vue";
import DmLoader from "@/components/ui-kit/DmLoader.vue";
import DmInput from "@/components/ui-kit/DmInput.vue";
import DmButton from "@/components/ui-kit/DmButton.vue";
import DmText from "@/components/ui-kit/DmText.vue";
import DmForm from "@/components/ui-kit/DmForm.vue";
import DmDropdown from "@/components/ui-kit/DmDropdown.vue";
import DmField from "@/components/ui-kit/DmField.vue";
import DmDialog from "@/components/ui-kit/DmDialog.vue";

export default function (application: App<Element>) {
  application.config.globalProperties.IconType = IconType;

  application
    .component("DmIcon", DmIcon)
    .component("DmLoader", DmLoader)
    .component("DmDialog", DmDialog)
    .component("DmForm", DmForm)
    .component("DmField", DmField);

  application
    .component("DmInput", DmInput)
    .component("DmText", DmText)
    .component("DmDropdown", DmDropdown)
    .component("DmButton", DmButton);

  application
    .component("PageTitle", PageTitle)
    .component("BlockTitle", BlockTitle)
    .component("SecondaryText", SecondaryText)
    .component("HumanDate", HumanDate)
    .component("HumanTimespan", HumanTimespan)
    .component("UserLink", UserLink);
}
