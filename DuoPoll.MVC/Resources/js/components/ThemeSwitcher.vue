<template>

  <div @click="">
    click
  </div>


</template>
<script>
export default {
  name: "ThemeSwitcher",
  data() {
    return {
      theme: "dark",
    }
  },
  methods: {
    switch() {
      if (this.theme === "dark") {
        this.theme = "light"
        document.documentElement.classList.remove('dark')
      } else {
        this.theme = "dark";
        document.documentElement.classList.add('dark')
      }
    },
  },

  mounted() {
    if (localStorage.theme === 'dark' || (!('theme' in localStorage) && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
      this.theme = "dark"
      document.documentElement.classList.add('dark')
    } else {
      this.theme = "light"
      document.documentElement.classList.remove('dark')
    }
  }
};
</script>
