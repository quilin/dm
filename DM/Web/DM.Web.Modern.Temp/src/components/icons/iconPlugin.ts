import type { App } from "vue";
import { IconType } from "@/components/icons/iconType";

export const iconPlugin = {
  install: (app: App) => (app.config.globalProperties.$iconType = IconType),
};

declare module "vue" {
  interface Vue {
    $iconType: typeof IconType;
  }
}
