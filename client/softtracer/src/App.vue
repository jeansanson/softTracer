<template>
  <v-app>
    <v-app-bar
      app
      color="white"
      elevation="1"
    >
    
      <h1>SoftTracer</h1>

      <v-spacer></v-spacer>

      <!-- TODO: Aqui irá ficar o botão de sair, apenas caso o usuário estiver logado -->
    </v-app-bar>

    <v-main>
      <Login/>
    </v-main>
  </v-app>
</template>

<script>
import Login from './components/Login';

export default {
  name: 'App',

  components: {
    Login,
  },

  data: () => ({
    //
  }),
};
</script>
