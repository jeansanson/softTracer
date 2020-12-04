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
          <v-list-item-title>requisitos aqui</v-list-item-title>
        </v-list-item-content>
      </v-list-item>

      <v-list dense>
        <v-list-item v-for="item in items" :key="item.title">
          <v-list-item-icon>
            <v-icon>{{ item.icon }}</v-icon>
          </v-list-item-icon>

          <v-list-item-content>
            <v-list-item-title>{{ item.title }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>
  </v-container>
</template>

<script>
export default {
  name: "Project Page",
};
</script>

<style></style>
