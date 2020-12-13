<template>
  <v-container>
    <div>
      <v-row class="mb-5 mt-2 ml-3">
        <v-icon class="mr-3" @click="goToProjects">mdi-arrow-left</v-icon>
        <h1>{{ name }}</h1>
      </v-row>
    </div>
    <Tasks />
    <v-navigation-drawer absolute right :width="350">
      <v-list-item>
        <v-list-item-content>
          <v-list-item-title class="font-weight-bold"
            >Resumo do projeto</v-list-item-title
          >
          <span class="resume">{{ resume }}</span>
        </v-list-item-content>
      </v-list-item>

      <v-divider></v-divider>
      <v-list-item>
        <v-list-item-content>
          <v-list-item-title class="font-weight-bold"
            >Data de criação</v-list-item-title
          >
          <v-list-item-title>{{ dateCreated }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
      <v-divider></v-divider>
      <v-list-item>
        <v-list-item-content>
          <v-list-item-title class="font-weight-bold"
            >Token do projeto</v-list-item-title
          >
          <v-list-item-title>{{ token }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
      <v-divider></v-divider>
      <v-list-item>
        <v-list-item-content>
          <v-list-item-title class="font-weight-bold"
            >Requisitos</v-list-item-title
          >
          <v-treeview
            v-if="requirements != null"
            hoverable
            activatable
            rounded
            :items="requirements"
          >
            <template v-slot:prepend="{ item }">
              <v-icon v-if="(item.completed = false)" color="success" dark
                >mdi-check</v-icon
              >
            </template>
          </v-treeview>
          <v-btn color="primary mt-3" @click="goToRequirements"
            >Visão detalhada</v-btn
          >
        </v-list-item-content>
      </v-list-item>
    </v-navigation-drawer>
  </v-container>
</template>

<script>
const axios = require("axios");
import Tasks from "../components/Tasks";

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
  components: {
    Tasks,
  },
  created: function() {
    if (this.$store.state.user_token == "") {
      this.goToLogin();
    } else {
      // faz o get do projeto pelo id, se der erro ele volta pra aba de projetos
      this.getProjectInfo();
      this.loadRequirements();
    }
  },
  methods: {
    getProjectInfo() {
      let self = this;

      this.id = this.$route.params.pathMatch;

      const URL = self.$store.state.apiURL + "/projects/" + this.id;

      const options = {
        headers: {
          Authorization: self.$store.state.user_token,
        },
      };

      axios
        .get(URL, options)
        .then(function(response) {
          self.dateCreated = self.convertDate(response.data.openingDate);
          self.resume = response.data.resume;
          self.name = response.data.name;
          self.token = response.data.token;
        })
        .catch(function(error) {
          if (error.response.data.Message !== "") {
            self.$snackbar.showMessage({
              content: error.response.data.message,
              color: "error",
            });
            self.goToLogin();
          }
        });
    },

    loadRequirements() {
      let self = this;
      const URL =
        self.$store.state.apiURL + "/projects/" + self.id + "/requirements";

      const options = {
        headers: {
          Authorization: self.$store.state.user_token,
        },
      };

      axios.get(URL, options).then((resp) => {
        self.requirements = resp.data;
      });
    },

    convertDate(string) {
      const date = new Date(string);
      return date.toLocaleDateString("pt-BR", { dateStyle: "long" });
    },

    // Route related methods
    goToProjects() {
      this.$router.push("/projects/");
    },

    goToLogin() {
      this.$router.push("/login");
    },

    goToRequirements() {
      this.$router.push("/projects/" + this.id + "/requirements");
    },
  },
};
</script>

<style>
.resume {
  word-wrap: break-word;
}
</style>
