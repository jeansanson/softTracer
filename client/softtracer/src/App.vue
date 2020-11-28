<template>
  <v-app>
    <v-app-bar
      app
      color="white"
      elevation="1"
    >
    
      <h1>SoftTracer</h1>

      <v-spacer></v-spacer>
      <v-btn elevation="2" color="primary" @click="login">Entrar</v-btn>
        <v-btn elevation="2" color="primary" class="ml-2" @click="register"
          >Cadastrar</v-btn
        >

      <!-- TODO: Aqui irá ficar o botão de sair, apenas caso o usuário estiver logado -->
    </v-app-bar>

    <v-main>
      <router-view></router-view>
    </v-main>
  </v-app>
</template>

<script>
//import Login from './components/Login';

export default {
  name: 'App',

  components: {
    //Login,
  },

  data: () => ({
    
  }),
  methods: {
    login() {
      this.$router.push("/login");
    },
    register() {
      this.$router.push("/register");
    },
  },
};
</script>
