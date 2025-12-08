import { computed, ref, useId } from "vue";

const activeId = ref<string | null>(null);

export default function useEditMode() {
  const id = useId();
  const isActive = computed(() => activeId.value === id);
  const acquire = () => (activeId.value = id);
  const release = () => (activeId.value = null);

  return {
    isActive,
    acquire,
    release,
  };
}
