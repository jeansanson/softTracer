<template>
  <v-app>
    <v-app-bar app color="white" elevation="2">
      <v-col class="text-left" cols="12">
        <h1>SoftTracer</h1>
      </v-col>
      <!-- TODO: Aqui irá ficar o botão de sair, apenas caso o usuário estiver logado -->
    </v-app-bar>

    <v-main>
      <router-view></router-view>
    </v-main>

    <v-footer padless>
      <v-col class="text-left" cols="12">
        {{ new Date().getFullYear() }} — <strong>SoftTracer</strong>
      </v-col>
    </v-footer>
    <Snackbar />
  </v-app>
</template>

<script>
import Snackbar from "./components/Snackbar";

export default {
  name: "App",

  components: {
    Snackbar,
  },

  data: () => ({}),
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
