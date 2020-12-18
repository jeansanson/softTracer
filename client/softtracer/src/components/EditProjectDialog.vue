<template>
  <v-row justify="center">
    <v-dialog v-model="dialog" persistent max-width="600px">
      <v-card>
        <v-card-title>
          <span class="headline font-weight-bold">{{ cardTitle }}</span>
          <v-icon></v-icon>
        </v-card-title>
        <v-card-text>
          <v-form ref="form" v-model="valid" lazy-validation>
            <v-text-field
              v-model="name"
              :rules="nameRules"
              :counter="255"
              label="Nome do projeto"
              required
            ></v-text-field>

            <v-textarea
              v-model="resume"
              :counter="4090"
              label="Resumo do projeto"
            >
            </v-textarea>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn color="blue darken-1" text type="submit">
            {{ cardButton }} </v-btn
          ><v-spacer></v-spacer>
          <v-btn color="error" text @click="deleteProject">
            <v-icon class="mr-2">mdi-delete</v-icon>
            Deletar
          </v-btn>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="close">
            Fechar
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-row>
</template>

<script>
const axios = require("axios");

export default {
  name: "EditProjectDialog",

  data: () => ({
    cardTitle: "",
    cardButton: "",
    dialog: false,
    valid: true,
    projectId: "",
    name: "",
    nameRules: [
      (v) => !!v || "Nome é um campo necessário",
      (v) => (v && v.length <= 255) || "Nome deve ter menos que 200 caracteres",
    ],
    resume: "",
  }),
  created() {
    this.$store.subscribe((mutation, state) => {
      if (mutation.type === "showEditProject") {
        this.cardTitle = "Editar projeto";
        this.cardButton = "Editar";
        this.projectId = state.currentProjectId;
        this.name = state.editProjectName;
        this.resume = state.editProjectResume;
        this.dialog = true;
      }
    });
  },
  methods: {
    close() {
      this.dialog = false;
      this.reset();
      this.resetValidation();
    },

    editProject() {
      let self = this;
      const URL = self.$store.state.apiURL + "/projects/" + self.projectId;

      const data = {
        id: self.taskId,
        name: self.name,
        resume: self.resume,
      };

      const options = {
        headers: {
          "Content-Type": "application/json",
          Authorization: self.$store.state.user_token,
        },
      };

      axios
        .put(URL, data, options)
        .then(function() {
          self.$snackbar.showMessage({
            content: "Tarefa editado com sucesso!",
            color: "success",
          });
          self.$store.commit("refreshTasks");
          self.close();
        })
        .catch(function(error) {
          console.log(error);
          if (error.response.data.message !== "") {
            self.$snackbar.showMessage({
              content: error.response.data.message,
              color: "error",
            });
          }
        });
    },

    deleteProject() {
      let self = this;
      const URL =
        self.$store.state.apiURL +
        "/projects/" +
        self.projectId;

      const options = {
        headers: {
          Authorization: self.$store.state.user_token,
        },
      };

      axios.delete(URL, options).then(() => {
        self.$snackbar.showMessage({
          content: "Projeto deletado com sucesso!",
          color: "success",
        });
        self.goToProjects();
        self.close();
      });
    },

    validate() {
      this.$refs.form.validate();
    },
    reset() {
      this.$refs.form.reset();
    },
    resetValidation() {
      this.$refs.form.resetValidation();
    },

    // Route related methods
    goToProjects() {
      this.$router.push("/projects/");
    },
  },
};
</script>
