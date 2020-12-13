<template>
  <v-container>
    <div>
      <v-row class="mb-5 mt-2 ml-3">
        <v-icon class="mr-3 mb-1" @click="goToProject">mdi-arrow-left</v-icon>
        <h1>Requisitos</h1>
        <v-spacer></v-spacer>
        <v-btn class="mt-4 mr-3" color="primary" @click="openCreateRequirement"
          >Adicionar requisito</v-btn
        >
      </v-row>
    </div>

    <v-treeview
      v-if="requirements != null"
      hoverable
      activatable
      rounded
      :items="requirements"
    >
      <template v-slot:prepend="{ item }">
        <v-icon v-if="(item.completed = false)" color="success" dark
          >mdi-check</v-icon
        >
      </template>
      <template v-slot:append="{ item }">
        
        <v-btn small rounded color="secondary" class="ma-2" @click="openEditRequirement(item)"
          ><v-icon small>mdi-pencil-outline</v-icon></v-btn
        >
        <v-btn
          small
          rounded
          color="error"
          class="ma-2"
          dark
          @click="removeRequirement(item)"
          ><v-icon small>mdi-close</v-icon></v-btn
        >
      </template>
    </v-treeview>

    <CreateRequirementDialog />
  </v-container>
</template>

<script>
const axios = require("axios");
import CreateRequirementDialog from "../components/CreateRequirementDialog";

export default {
  name: "Requirements",

  data: () => ({
    projectId: "",
    requirements: [],
  }),
  components: {
    CreateRequirementDialog,
  },
  created: function() {
    if (this.$store.state.user_token == "") {
      this.$router.push("/login");
    } else {
      this.projectId = this.$route.params.pathMatch;
      this.loadRequirements();
      this.$store.subscribe((mutation) => {
        if (mutation.type === "refreshRequirements") {
          this.loadRequirements();
        }
      });
    }
  },
  methods: {
    loadRequirements() {
      let self = this;
      const URL =
        self.$store.state.apiURL +
        "/projects/" +
        self.projectId +
        "/requirements";

      const options = {
        headers: {
          Authorization: self.$store.state.user_token,
        },
      };

      axios.get(URL, options).then((resp) => {
        self.requirements = resp.data;
      });
    },

    openCreateRequirement() {
      this.$store.commit("showCreateRequirement", {
        projectId: this.projectId,
        requirements: this.requirements,
        editRequirement: "", // passar o requisito pra editar
      });
    },

    openEditRequirement(item) {
      this.$store.commit("showEditRequirement", {
        projectId: this.projectId,
        requirements: this.requirements,
        editRequirement: item,
      });
    },

    visualizeRequirement(item) {
      console.log(item);
    },

    removeRequirement(item) {
      let self = this;
      const URL =
        self.$store.state.apiURL +
        "/projects/" +
        self.projectId +
        "/requirements/" +
        item.id;

      const options = {
        headers: {
          Authorization: self.$store.state.user_token,
        },
      };

      axios.delete(URL, options).then(() => {
        self.$snackbar.showMessage({
          content: "Requisito deletado com sucesso!",
          color: "success",
        });
        self.loadRequirements();
      });
    },

    // Route related methods
    goToProject() {
      this.$router.push("/projects/" + this.projectId);
    },
  },
};
</script>

<style></style>
