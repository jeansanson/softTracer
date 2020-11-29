<template>
  <div class="home">
    
    <v-container>
      <h1>Projects</h1>
    </v-container>
  </div>
</template>

<script>
// @ is an alias to /src

export default {
  name: "Home",
  components: {},
  methods: {},
  created: function () {
    this.$router.push("/login");
  }
};
</script>
