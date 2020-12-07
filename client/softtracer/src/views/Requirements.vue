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
      :items="requirements"
    ></v-treeview>

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
    requirements: "",
  }),
  components: {
    CreateRequirementDialog,
  },
  created: function() {
    if (this.$store.state.user_token == "") {
      this.$router.push("/login");
    } else {
      this.projectId = this.$route.params.pathMatch;
      // faz o get do projeto pelo id, se der erro ele volta pra aba de projetos
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
        var obj = resp.data;
        obj = obj.map(function(item) {
          for (var key in item) {
            item[key.toLowerCase()] = item[key];
            delete item[key];
          }
          return item;
        });
        console.log(obj);
      });
    },

    openCreateRequirement() {
      this.$store.commit("showCreateRequirement", {
        projectId: this.projectId,
        requirements: this.requirements,
        editRequirement: "", // passar o requisito pra editar
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
