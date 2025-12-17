import { defineStore } from "pinia";
import { ref } from "vue";
import { ColorSchema } from "@/api/models/community";
import { useUserStore } from "@/stores/user";
import i18n from "@/i18n";
import dayjs from "dayjs";

export const useUiStore = defineStore("ui", () => {
  const theme = ref(ColorSchema.Modern);

  const userStore = useUserStore();

  const updateTheme = (newTheme: ColorSchema) => (theme.value = newTheme);
  const toggleTheme = () => {
    if (theme.value !== ColorSchema.Night) {
      updateTheme(ColorSchema.Night);
      return;
    }

    const schema = userStore.user?.settings?.colorSchema ?? ColorSchema.Modern;
    updateTheme(schema === ColorSchema.Night ? schema : ColorSchema.Modern);
  };

  const toggleLocale = () => {
    if (i18n.global.locale.value === "ru-RU") {
      i18n.global.locale.value = "en-US";
    } else {
      i18n.global.locale.value = "ru-RU";
    }
  };

  return { theme, updateTheme, toggleTheme, toggleLocale };
});
