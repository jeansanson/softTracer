<template>
  <v-row justify="center">
    <v-dialog v-model="dialog" persistent max-width="600px">
      <v-card>
        <v-card-title>
          <span class="headline font-weight-bold">Criar projeto</span>
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
              :rules="resumeRules"
              :counter="4090"
              required
              label="Resumo do projeto"
            >
            </v-textarea>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="close">
            Fechar
          </v-btn>
          <v-btn
            color="blue darken-1"
            text
            @click="createProject"
            type="submit"
          >
            Criar
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-row>
</template>

<script>
const axios = require("axios");

export default {
  name: "CreateProjectDialog",

  data: () => ({
    dialog: false,
    valid: true,
    name: "",
    nameRules: [
      (v) => !!v || "Nome é um campo necessário",
      (v) => (v && v.length <= 255) || "Nome deve ter menos que 100 caracteres",
    ],
    resume: "",
    resumeRules: [
      (v) => !!v || "Resumo é um campo necessário",
      (v) =>
        (v && v.length <= 4090) || "Resumo deve ter menos que 4090 caracteres",
    ],
  }),
  created() {
    this.$store.subscribe((mutation) => {
      if (mutation.type === "showCreateProject") {
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

    createProject() {
      let self = this;
      const URL = self.$store.state.apiURL + "/projects";

      const data = {
        name: this.name,
        resume: this.resume,
      };

      const options = {
        headers: {
          Authorization: self.$store.state.user_token,
        },
      };

      axios
        .post(URL, data, options)
        .then(function() {
          self.$snackbar.showMessage({
            content: "Projeto criado com sucesso!",
            color: "success",
          });
          self.$store.commit("refreshProjects");
          self.close();
        })
        .catch(function(error) {
          console.log(error);
          if (error.response.data.Message !== "") {
            self.$snackbar.showMessage({
              content: error.response.data.Message,
              color: "error",
            });
          }
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
  },
};
</script>
