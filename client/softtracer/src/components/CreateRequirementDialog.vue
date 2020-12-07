<template>
  <v-row justify="center">
    <v-dialog v-model="dialog" persistent max-width="600px">
      <v-card>
        <v-card-title>
          <span class="headline font-weight-bold">Criar requisito</span>
        </v-card-title>
        <v-card-text>
          <v-form ref="form" v-model="valid" lazy-validation>
            <v-text-field
              v-model="name"
              :rules="nameRules"
              :counter="200"
              label="Nome do requisito"
              required
            ></v-text-field>

            <v-textarea
              v-model="description"
              :rules="descriptionRules"
              :counter="4000"
              required
              label="Descrição do requisito"
            >
            </v-textarea>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="close">
            Fechar
          </v-btn>
          <v-btn
            color="blue darken-1"
            text
            @click="createRequirement"
            type="submit"
          >
            Criar
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-row>
</template>

<script>
const axios = require("axios");

export default {
  name: "CreateRequirementDialog",

  data: () => ({
    dialog: false,
    valid: true,
    projectId: "",
    name: "",
    nameRules: [
      (v) => !!v || "Nome é um campo necessário",
      (v) => (v && v.length <= 200) || "Nome deve ter menos que 200 caracteres",
    ],
    description: "",
    descriptionRules: [
      (v) => !!v || "Descrição é um campo necessário",
      (v) =>
        (v && v.length <= 4000) ||
        "Descrição deve ter menos que 4000 caracteres",
    ],
  }),
  created() {
    this.$store.subscribe((mutation, state) => {
      if (mutation.type === "showCreateRequirement") {
        this.projectId = state.currentProjectId;
        
        this.dialog = true;
      }
    });
  },
  methods: {
    close() {
      this.dialog = false;
      this.reset();
      this.resetValidation();
    },

    createRequirement() {
      let self = this;
      const URL =
        self.$store.state.apiURL +
        "/projects/" +
        self.projectId +
        "/requirements";

      const data = [
        {
          Id: 10,
          Name: self.name,
          Description: self.description,
        },
      ];

      const options = {
        headers: {
          "Content-Type": "application/json",
          Authorization: self.$store.state.user_token,
        },
      };

      axios
        .put(URL, data, options)
        .then(function() {
          self.$snackbar.showMessage({
            content: "Requisito adicionado com sucesso!",
            color: "success",
          });
          self.$store.commit("refreshRequirements");
          self.close();
        })
        .catch(function(error) {
          console.log(error);
          if (error.response.data.Message !== "") {
            self.$snackbar.showMessage({
              content: error.response.data.Message,
              color: "error",
            });
          }
        });
    },

    validate() {
      this.$refs.form.validate();
    },
    reset() {
      this.$refs.form.reset();
    },
    resetValidation() {
      this.$refs.form.resetValidation();
    },
  },
};
</script>
