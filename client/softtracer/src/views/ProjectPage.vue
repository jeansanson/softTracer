<template>
  <v-container>
    <div>
      <v-row class="mb-5 mt-2 ml-3">
        <v-icon class="mr-3" @click="goToProjects">mdi-arrow-left</v-icon>
        <h1>Nome do projeto</h1>
      </v-row>
    </div>
    <v-navigation-drawer absolute permanent right width="350">
      <template v-slot:prepend>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-title class="font-weight-bold"
              >Resumo do projeto</v-list-item-title
            >
            <v-list-item-title>resumo aqui</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </template>
      <v-divider></v-divider>
      <v-list-item>
        <v-list-item-content>
          <v-list-item-title class="font-weight-bold"
            >Data de criação</v-list-item-title
          >
          <v-list-item-title>data aqui</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
      <v-divider></v-divider>
      <v-list-item>
        <v-list-item-content>
          <v-list-item-title class="font-weight-bold"
            >Requisitos</v-list-item-title
          >
          <v-btn color="primary mt-3" @click="goToRequirements">Visão detalhada</v-btn>
        </v-list-item-content>
      </v-list-item>
    </v-navigation-drawer>
  </v-container>
</template>

<script>
export default {
  name: "ProjectPage",

  data: () => ({
    id: "",
    requirements: [],
    dateCreated: "",
    resume: "",
    name: "",
    tasks: [],
    token: "",
  }),
  created: function() {
    if (this.$store.state.user_token == "") {
      this.$router.push("/login");
    } else {
      this.id = this.$route.params.pathMatch;
      // faz o get do projeto pelo id, se der erro ele volta pra aba de projetos
    }
  },
  methods: {
    // Route related methods
    goToProjects() {
      this.$router.push("/projects/");
    },

    goToRequirements() {
      this.$router.push("/projects/" + this.id + "/requirements");
    },
  },
};
</script>

<style></style>
