<template>
  <v-container>
    <h1>Projects</h1>
  </v-container>
</template>

<script>
export default {
  name: "Projects",

  data: () => ({}),
  created: function () {
    if(this.$store.state.user_token == ""){
      this.$router.push("/login");
    } else {
      console.log("logado!");
    }
  }
}
</script>

<style>

</style>