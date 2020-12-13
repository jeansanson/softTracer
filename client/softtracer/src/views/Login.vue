<template>
  <v-container>
    <v-row class="text-center">
      <v-col cols="12" sm="4"></v-col>
      <v-col cols="12" sm="4">
        <h1 class="display-1 font-weight-bold mb-3">
          Login
        </h1>

        <v-form ref="form" v-model="valid" lazy-validation>
          <v-text-field
            v-model="user"
            :rules="userRules"
            label="Usuário"
            required
          ></v-text-field>

          <v-text-field
            v-model="password"
            :rules="passwordRules"
            label="Senha"
            type="password"
            required
          ></v-text-field>

          <v-btn
            :disabled="!valid"
            color="primary"
            class="mr-4"
            @click="validate"
            type="submit"
          >
            Entrar
          </v-btn>

          <v-btn elevation="2" color="secondary" @click="goToRegister"
            >Criar uma conta</v-btn
          >
        </v-form>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
const axios = require("axios");

export default {
  name: "Login",

  data: () => ({
    valid: true,
    user: "",
    userRules: [
      (v) => !!v || "Usuário é um campo necessário",
      (v) =>
        (v && v.length <= 100) || "Usuário deve ter menos que 100 caracteres",
    ],
    password: "",
    passwordRules: [
      (v) => !!v || "Senha é um campo necessário",
      (v) =>
        (v && v.length <= 100) || "Senha deve ter menos que 100 caracteres",
    ],
    error: "",
  }),
  methods: {
    login() {
      let self = this;
      const URL = self.$store.state.apiURL + "/users/authentication";

      const data = {
        UserId: this.user,
        password: this.password,
      };

      const options = {
        headers: {
          "Content-Type": "application/json",
        },
      };

      axios
        .post(URL, data, options)
        .then(function(response) {
          self.$store.commit("storeLogin", response.data);
          self.$snackbar.showMessage({
            content: "Login realizado com sucesso!",
            color: "success",
          });
          self.goToProjects();
        })
        .catch(function(error) {
          console.log(error.response.data.message);
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

      if (this.valid) {
        this.login();
      }
    },

    // Route related methods
    goToRegister() {
      this.$router.push("/register");
    },

    goToProjects() {
      this.$router.push("/projects");
    },
  },
  created: function() {
    if (this.$store.state.user_token !== "") {
      this.goToProjects();
    }
  },
};
</script>
