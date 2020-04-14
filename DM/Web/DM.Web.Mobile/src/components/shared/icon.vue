<template>
  <span class="base-unselectable iconic" :class="mod" v-html="charCode"></span>
</template>

<script type="text/javascript">
  const icons = {
    'sidebar': 0xe01d,
    'search': 0xe009,
    'comment': 0xe05d,
    'user': 0xe020,
    'message': 0xe011,
    'close': 0xe00e,
    'like': 0xe077,

    'corner-top': 0xe04d,
    'corner-right': 0xe04a,
    'corner-bottom': 0xe04c,
    'corner-left': 0xe04b,
    'arrow-up': 0xe042,
    'arrow-right': 0xe041,
    'arrow-down': 0xe040,
    'arrow-left': 0xe04f,
    'backward': 0xe051,
    'forward': 0xe050,

    'posts-unread': 0xe024,
    'posts-no-unread': 0xe064,
    'comments-unread': 0xe05d,
    'comments-no-unread': 0xe054,

    'post-vote-positive': 0xe00d,
    'post-vote-neutral': 0xe05d,
    'post-vote-negative': 0xe01e,

    'list': 0xe01c,

    'logout': 0xe039,
    'home': 0xe05c,

    'edit': 0xe00f,
    'closed': 0xe02e,
    'attached': 0xe073,

    'blast': 0xe076,
    'hidden': 0xe043,
    'fair': 0xe038
  }

  export default {
    props: ['font', 'mod'],

    created () {
      if (!(this.font in icons)) {
        throw new Error(`No such font as ${this.font}`)
      }
    },

    computed: {
      charCode () {
        return '&#x' + icons[this.font].toString(16) + ';'
      }
    }
  }
</script>