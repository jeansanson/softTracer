<template>
  <v-container>
    <v-row class="text-center">
      <v-col cols="12" sm="4"></v-col>
      <v-col cols="12" sm="4">
        <h1 class="display-1 font-weight-bold mb-3">
          Crie uma conta
        </h1>

        <v-form ref="form" v-model="valid" lazy-validation>
          <v-text-field
            v-model="user"
            :counter="100"
            :rules="userRules"
            label="Usuário"
            required
          ></v-text-field>

          <v-text-field
            v-model="name"
            :counter="100"
            :rules="nameRules"
            label="Nome"
            required
          ></v-text-field>

          <v-text-field
            v-model="email"
            :rules="emailRules"
            label="E-mail"
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
            Cadastrar
          </v-btn>

          <v-btn elevation="2" color="secondary" @click="goToLogin"
            >Voltar</v-btn
          >
        </v-form>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
const axios = require("axios");

export default {
  name: "Register",

  data: () => ({
    valid: true,
    user: "",
    userRules: [
      (v) => !!v || "Usuário é um campo necessário",
      (v) =>
        (v && v.length <= 100) || "Usuário deve ter menos que 100 caracteres",
    ],
    name: "",
    nameRules: [
      (v) => !!v || "Nome é um campo necessário",
      (v) => (v && v.length <= 100) || "Nome deve ter menos que 100 caracteres",
    ],
    email: "",
    emailRules: [
      (v) => !!v || "E-mail é um campo necessário",
      (v) => /.+@.+\..+/.test(v) || "O e-mail precisa ser válido",
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
    register() {
      let self = this;
      const URL = self.$store.state.apiURL + "/users";

      const data = {
        userId: this.user,
        displayname: this.name,
        email: this.email,
        password: this.password,
      };

      axios
        .post(URL, data)
        .then(function() {
          self.$snackbar.showMessage({
            content: "Usuário cadastrado com sucesso!",
            color: "success",
          });
          self.goToLogin();
        })
        .catch(function(error) {
          if (error.response.data.Message !== "") {
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
        this.register();
      }
    },

    // Route related methods
    goToLogin() {
      this.$router.push("/login");
    },

    goToProjects() {
      this.$router.push("/projects");
    }
  },
  created: function() {
    if (this.$store.state.user_token !== "") {
      this.goToProjects();
    }
  },
};
</script>

<style></style>
