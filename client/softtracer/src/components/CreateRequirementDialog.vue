<template>
  <v-row justify="center">
    <v-dialog v-model="dialog" persistent max-width="600px">
      <v-card>
        <v-card-title>
          <span class="headline font-weight-bold">{{ cardTitle }}</span>
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

            <v-select
              v-model="select"
              :items="requirements"
              label="Requisito pai"
              item-text="name"
              item-value="id"
            ></v-select>

            <v-checkbox
              v-model="checkbox"
              label="Completo"
            ></v-checkbox>

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
            {{ cardButton }}
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
    editable: false,
    select: null,
    checkbox: false,
    cardTitle: "",
    cardButton: "",
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
    requirements: [],
  }),
  created() {
    this.$store.subscribe((mutation, state) => {
      if (mutation.type === "showCreateRequirement") {
        this.cardTitle = "Adicionar requisito";
        this.cardButton = "Criar";
        this.projectId = state.currentProjectId;
        this.copyRequirementsObject(state.requirements);
        (this.editable = false), (this.dialog = true);
      } else if (mutation.type === "showEditRequirement") {
        this.cardTitle = "Editar requisito";
        this.cardButton = "Editar";
        this.name = state.editRequirement.name;
        this.select = state.editRequirement.parentId;
        this.checkbox = state.editRequirement.completed;
        this.description = state.editRequirement.description;
        this.projectId = state.currentProjectId;
        this.copyRequirementsObject(state.requirements);
        this.editable = true;
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

    copyRequirementsObject(requirements) {
      // copia o objeto de requirements
      let key;
      for (key in requirements) {
        this.requirements[key] = requirements[key]; // copies each property to the objCopy object
      }
      this.requirements.unshift({ id: 0, name: "Nenhum" });
    },

    createRequirement() {
      let self = this;
      const URL =
        self.$store.state.apiURL +
        "/projects/" +
        self.projectId +
        "/requirements";

      if (this.editable == false) {
        const data = [
          {
            Name: self.name,
            Description: self.description,
            parentId: self.select,
            completed: self.checkbox
          },
        ];

        const options = {
          headers: {
            "Content-Type": "application/json",
            Authorization: self.$store.state.user_token,
          },
        };

        axios
          .post(URL, data, options)
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
                content: error.response.data.message,
                color: "error",
              });
            }
          });
      } else if (this.editable == true) {
        if (
          this.$store.state.editRequirement.children.length !== 0 &&
          this.select !== 0
        ) {
          self.$snackbar.showMessage({
            content:
              "Não é possível alterar o requisito pai enquanto possuir requisitos-filho",
            color: "error",
          });
        } else if (this.$store.state.editRequirement.id === this.select) {
          self.$snackbar.showMessage({
            content: "Não é possível adicionar o próprio requisito como pai",
            color: "error",
          });
        } else {
          const data = [
            {
              Id: self.$store.state.editRequirement.id,
              Name: self.name,
              Description: self.description,
              parentId: self.select,
              completed: self.checkbox
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
                content: "Requisito editado com sucesso!",
                color: "success",
              });
              self.$store.commit("refreshRequirements");
              self.close();
            })
            .catch(function(error) {
              console.log(error);
              if (error.response.data.Message !== "") {
                self.$snackbar.showMessage({
                  content: error.response.data.message,
                  color: "error",
                });
              }
            });
        }
      }
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
