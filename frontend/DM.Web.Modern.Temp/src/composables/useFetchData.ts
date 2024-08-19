import { type RouteParams, useRoute } from "vue-router";
import { watch, onMounted } from "vue";

type FetchDataStrategy = {
  param: (params: RouteParams) => string | string[];
  callback: (param: string | string[]) => void;
};

export function useFetchData(
  mountHook: () => any,
  strategies: FetchDataStrategy[],
) {
  onMounted(mountHook);

  const route = useRoute();
  watch(
    strategies.map((s) => () => s.param(route.params)),
    (oldParams, newParams) => {
      for (let i = 0; i < oldParams.length; i++) {
        if (oldParams[i] !== newParams[i]) {
          strategies[i].callback(newParams[i]);
          break;
        }
      }
    },
    {
      flush: "post",
    },
  );
}
