<template>
  <v-container>
    <v-row class="text-center">
      <v-col cols="12" sm="4"></v-col>
      <v-col cols="12" sm="4">
        <h1 class="display-1 font-weight-bold mb-3">
          Login
        </h1>

        <v-text-field label="Usuário"></v-text-field>
        <v-text-field
          label="Senha"
          :type="show1 ? 'text' : 'password'"
        ></v-text-field>
        <v-btn elevation="2" color="primary">Entrar</v-btn>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  name: "Login",

  data: () => ({}),
};
</script>
