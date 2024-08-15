import { defineStore } from "pinia";
import { ref } from "vue";
import { ColorSchema } from "@/api/models/community";
import { useUserStore } from "@/stores/user";

export const useUiStore = defineStore("ui", () => {
  const theme = ref(ColorSchema.Modern);

  const { user } = useUserStore();

  const updateTheme = (newTheme: ColorSchema) => (theme.value = newTheme);
  const toggleTheme = () => {
    if (theme.value !== ColorSchema.Night) {
      updateTheme(ColorSchema.Night);
      return;
    }

    const schema = user?.settings?.colorSchema ?? ColorSchema.Modern;
    updateTheme(schema === ColorSchema.Night ? schema : ColorSchema.Modern);
  };

  return { theme, updateTheme, toggleTheme };
});
