<template>
  <v-container>
    <h1>Nome do projeto</h1>
    <v-navigation-drawer absolute permanent right width="350">
      <template v-slot:prepend>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-title class="font-weight-bold"
              >Resumo do projeto</v-list-item-title
            >
            <v-list-item-title>resumo aqui</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </template>

      <v-divider></v-divider>

      <v-list-item>
        <v-list-item-content>
          <v-list-item-title class="font-weight-bold"
            >Data de criação</v-list-item-title
          >
          <v-list-item-title>data aqui</v-list-item-title>
        </v-list-item-content>
      </v-list-item>

      <v-divider></v-divider>

      <v-list-item>
        <v-list-item-content>
          <v-list-item-title class="font-weight-bold"
            >Requisitos</v-list-item-title
          >
          <v-btn color="primary">Adicionar requisito</v-btn>
        </v-list-item-content>
      </v-list-item>
    </v-navigation-drawer>
  </v-container>
</template>

<script>
export default {
  name: "ProjectPage",

  data: () => ({
    requirements: [],
  }),
  created: function() {
    if (this.$store.state.user_token == "") {
      this.$router.push("/login");
    } else {
      // faz o get do projeto pelo id, se der erro ele volta pra aba de projetos
    }
  },
};
</script>

<style></style>
