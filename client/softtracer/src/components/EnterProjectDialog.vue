<template>
  <v-row justify="center">
    <v-dialog v-model="dialog" persistent max-width="600px">
      <v-card>
        <v-card-title>
          <span class="headline font-weight-bold">Entrar em um projeto</span>
        </v-card-title>
        <v-card-text>
          <v-form ref="form" v-model="valid" lazy-validation>
            <v-text-field
              v-model="token"
              :rules="tokenRules"
              label="Token"
              required
            ></v-text-field
          ></v-form>
        </v-card-text>
        <v-card-actions
          ><v-btn
            color="blue darken-1"
            text
            type="submit"
            @click="enterProject()"
          >
            Entrar
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
  name: "CreateProjectDialog",

  data: () => ({
    dialog: false,
    valid: true,
    token: "",
    tokenRules: [(v) => !!v || "Token é um campo necessário"],
  }),
  created() {
    this.$store.subscribe((mutation) => {
      if (mutation.type === "showEnterProject") {
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

    enterProject() {
      let self = this;
      const URL = self.$store.state.apiURL + "/projects/users";

      const data = {
        ProjectToken: this.token
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
            content: "Projeto adicionado com sucesso!",
            color: "success",
          });
          self.$store.commit("refreshProjects");
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
