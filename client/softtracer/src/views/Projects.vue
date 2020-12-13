<template>
  <v-container>
    <v-row>
      <h1 class="mb-5 mt-2 ml-3">Seus projetos</h1>
      <v-spacer></v-spacer>
      <v-btn class="mt-4 mr-3" color="primary" @click="openEnterProject">Entrar em um projeto</v-btn>
      <v-btn class="mt-4 mr-3" color="primary" @click="openCreateProject"
        >Criar projeto</v-btn
      >
    </v-row>
    <v-row dense>
      <v-col v-for="project in projects" :key="project.id" :cols="cols">
        <v-card @click="loadProject(project)" height="190px">
          <v-card-title
            v-text="project.name"
            class="justify-center project-title"
          ></v-card-title>
        </v-card>
      </v-col>
    </v-row>

    <EnterProjectDialog />
    <CreateProjectDialog />
  </v-container>
</template>

<script>
const axios = require("axios");
import CreateProjectDialog from "../components/EnterProjectDialog";
import EnterProjectDialog from "../components/CreateProjectDialog";

export default {
  name: "Projects",

  components: {
    CreateProjectDialog,
    EnterProjectDialog
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
  computed: {
    cols() {
      switch (this.$vuetify.breakpoint.name) {
        case "xs":
          return 6;
        case "sm":
          return 4;
        case "md":
          return 4;
        case "lg":
          return 3;
        case "xl":
          return 2;
        default:
          return 3;
      }
    },
  },
  methods: {
    loadProjects() {
      let self = this;
      const URL = self.$store.state.apiURL + "/projects";

      const options = {
        headers: {
          Authorization: self.$store.state.user_token,
        },
      };

      axios.get(URL, options).then((resp) => {
        self.projects = resp.data;
      });
    },

    loadProject(project) {
      this.$router.push("/projects/" + project.id);
    },

    openCreateProject() {
      this.$store.commit("showCreateProject");
    },

    openEnterProject() {
      this.$store.commit("showEnterProject");
    },
  },
};
</script>

<style>
.project-title {
  height: 175px;
  overflow: hidden;
  white-space: pre-line;
  word-break: keep-all;
  text-align: center;
}
</style>
