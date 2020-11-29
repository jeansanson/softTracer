<template>
  <v-container>
    <h1>Seus projetos</h1>
    <v-fab-transition>
      <v-btn
        color="primary"
        dark
        absolute
        bottom
        right
        fab
        @click="openCreateProject"
      >
        <v-icon>mdi-plus</v-icon>
      </v-btn>
    </v-fab-transition>
    <CreateProjectDialog/>
  </v-container>
</template>

<script>
import CreateProjectDialog from "../components/CreateProjectDialog";

export default {
  name: "Projects",

  components: {
    CreateProjectDialog
  },
  data: () => ({}),
  created: function() {
    if (this.$store.state.user_token == "") {
      this.$router.push("/login");
    } else {
      // carregar os projetos
    }
  },
  methods: {

    // abre o popup para criar projetos
    openCreateProject() {
      this.$store.commit("showCreateProject");
    },
  },
};
</script>

<style></style>
