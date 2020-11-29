<template>
  <v-container>
    <v-row>
      <h1 class="mb-5 mt-2 ml-3">Seus projetos</h1>
      <v-spacer></v-spacer>
      <v-btn class="mt-4 mr-3" color="primary" @click="openCreateProject"
        >Criar projeto</v-btn
      >
    </v-row>
    <v-row dense>
      <v-col v-for="project in projects" :key="project.Id" :cols="2">
        <v-card @click="console.log('')" height="190px">
          <v-card-title
            v-text="project.Name"
            class="justify-center"
          ></v-card-title>
        </v-card>
      </v-col>
    </v-row>

    <!-- <v-fab-transition>
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
    </v-fab-transition> -->
    <CreateProjectDialog />
  </v-container>
</template>

<script>
const axios = require("axios");
import CreateProjectDialog from "../components/CreateProjectDialog";

export default {
  name: "Projects",

  components: {
    CreateProjectDialog,
  },
  data: () => ({ projects: [] }),
  created: function() {
    if (this.$store.state.user_token == "") {
      this.$router.push("/login");
    } else {
      this.loadProjects();
      this.$store.subscribe((mutation) => {
      if (mutation.type === "refreshProjects") {
        this.loadProjects();
      }
    });
    }
  },
  methods: {
    loadProjects() {
      let self = this;
      const URL = "https://localhost:44342/api/projects";

      const options = {
        headers: {
          "Access-Control-Allow-Origin": "*",
          "Access-Control-Allow-Methods": "DELETE, POST, GET, OPTIONS",
          "Access-Control-Allow-Headers":
            "Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With",
          "Content-Type": "application/json",
          Authorization: self.$store.state.user_token,
        },
      };

      axios.get(URL, options).then((resp) => {
        //this.results = resp.data.results;
        //const entries = Object.keys(resp.data.results.stocks);
        //const stocks = [];
        console.log(resp.data);
        self.projects = resp.data;
        // entries.forEach((element) => {
        //   const stock = resp.data.results.stocks[element];
        //   stocks.push({
        //     title: element,
        //     location: stock.location,
        //     value: stock.variation,
        //   });
        // });
        // this.stocks = stocks;
        // console.log(this.stocks);
      });
    },

    // abre o popup para criar projetos
    openCreateProject() {
      this.$store.commit("showCreateProject");
    },
  },
};
</script>

<style></style>
