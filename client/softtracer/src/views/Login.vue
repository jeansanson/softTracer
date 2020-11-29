<template>
  <v-container>
    <v-row class="text-center">
      <v-col cols="12" sm="4"></v-col>
      <v-col cols="12" sm="4">
        <h1 class="display-1 font-weight-bold mb-3">
          Login
        </h1>

        <v-form ref="form" v-model="valid" lazy-validation >
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
      const URL = "https://localhost:44342/api/users/authentication";

      const data = {
        UserId: this.user,
        password: this.password,
      };

      const options = {
        headers: {
          "Access-Control-Allow-Origin": "*",
          "Access-Control-Allow-Methods": "DELETE, POST, GET, OPTIONS",
          "Access-Control-Allow-Headers":
            "Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With",
          "Content-Type": "application/json",
        },
      };

      let self = this;

      axios
        .post(URL, data, options)
        .then(function(response) {
          console.log(response.data);
          self.$store.commit("storeLogin", response.data);
          self.$snackbar.showMessage({ content: "Login realizado com sucesso!", color: "success" });
          self.goToProjects();
        })
        .catch(function(error) {
          console.log(error.response.data.Message);
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
    }
  },
};
</script>
