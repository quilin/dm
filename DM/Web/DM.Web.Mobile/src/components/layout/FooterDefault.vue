<template>
  <span class="footer-icons">
    <router-link :to="{ name: 'home' }"><icon font="home"/></router-link>
    <icon font="sidebar" @click.native="changeSidebar" />
  </span>
</template>

<script>
  import Icon from '../shared/icon'
  import {mapGetters} from "vuex";

  export default {
    name: "FooterDefault",
    components: {Icon},
    computed: {
      ...mapGetters({
        sidebarShow: 'getInterfaceSidebarShow',
      })
    },
    methods: {
      changeSidebar() {
        this.$store.dispatch('setInterfaceSidebarShow', {status: !this.sidebarShow});
      }
    }
  }
</script>

<style scoped lang="less">
  .footer-icons {
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
