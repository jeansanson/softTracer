<template>
  <v-app>
    <v-app-bar app color="white" elevation="2">
      <v-col class="text-left" cols="10">
        <h1>SoftTracer</h1>
      </v-col>

      <v-spacer></v-spacer>
      
      <div class="mr-1" v-if="this.$store.state.user_token !== ''">
        <v-icon>mdi-account-circle</v-icon>
      </div>
      <h4 class="mr-5" >{{ this.$store.state.user_name }}</h4>
      <div class="mr-2" v-if="this.$store.state.user_token !== ''">
        <v-row>
          <v-btn elevation="0" @click="logout" >
            <h4 class="mr-2">Sair</h4>
            <v-icon>mdi-exit-to-app</v-icon>
          </v-btn>
        </v-row>
      </div>
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
    logout() {
      this.$store.commit("logout");
      this.goTologin();
    },

    // Route related methods
    goTologin() {
      this.$router.push("/login");
    },
  },
};
</script>
