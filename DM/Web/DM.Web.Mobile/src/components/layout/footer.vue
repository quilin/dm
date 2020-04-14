<template>
  <span class="sidebar-icons">
    <icon font="sidebar"/>
    <router-link :to="{ name: 'home' }" ><icon font="home"/></router-link>
  </span>
</template>

<script>
  import Icon from '../shared/icon'

  export default {
    name: "sidebar-default",
    components: {Icon}
  }
</script>

<style scoped lang="less">
.sidebar-icons {
  display: flex;
  align-items: center;
  justify-content: space-between;
  height: 3rem;
  padding: 0 1rem;

  .iconic {
    font-size: 2rem;
  }
}
</style>
