<template>
  <v-row justify="center">
    <v-dialog v-model="dialog" persistent max-width="600px">
      <v-card>
        <v-card-title>
          <span class="headline font-weight-bold">{{ cardTitle }}</span>
          <v-icon></v-icon>
        </v-card-title>
        <v-card-text>
          <v-form ref="form" v-model="valid" lazy-validation>
            <v-text-field
              v-model="name"
              :rules="nameRules"
              :counter="255"
              label="Nome da tarefa"
              required
            ></v-text-field>

            <v-text-field
              v-model="responsible"
              label="Responsáveis (separado por vírgula)"
            ></v-text-field>

            <v-select
              v-model="select"
              :items="requirements"
              label="Requisito associado"
              item-text="name"
              item-value="id"
            ></v-select>

            <v-textarea
              v-model="description"
              :counter="4090"
              label="Descrição da tarefa"
            >
            </v-textarea>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn color="blue darken-1" text @click="editTask" type="submit">
            {{ cardButton }} </v-btn
          ><v-spacer></v-spacer>
          <v-btn color="error" text @click="deleteTask">
            <v-icon class="mr-2">mdi-delete</v-icon>
            Deletar
          </v-btn>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="close">
            Fechar
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-row>
</template>

<script>
const axios = require("axios");

export default {
  name: "EditTaskDialog",

  data: () => ({
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
      (v) => (v && v.length <= 255) || "Nome deve ter menos que 200 caracteres",
    ],
    description: "",
    responsible: "",
    requirements: [],
    stage: 0,
    taskId: null,
  }),
  created() {
    this.$store.subscribe((mutation, state) => {
      if (mutation.type === "showEditTask") {
        this.cardTitle = "Editar tarefa";
        this.cardButton = "Editar";
        this.projectId = state.currentProjectId;
        this.copyRequirementsObject(state.requirements);
        this.taskId = state.selectedTask.id;
        this.stage = state.selectedTask.stage;
        this.description = state.selectedTask.description;
        this.select = state.selectedTask.requirementId;
        this.name = state.selectedTask.name;
        this.responsible = state.selectedTask.responsibles.join(", ");
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

    editTask() {
      let self = this;
      const URL =
        self.$store.state.apiURL +
        "/projects/" +
        self.projectId +
        "/tasks/" +
        self.taskId;

      var responsibles = [];
      self.responsible = self.responsible.replace(" ", "");
      if (self.responsible.includes(",")) {
        responsibles = self.responsible.split(",");
      } else {
        responsibles.push(self.responsible);
      }

      

      const data = {
        id: self.taskId,
        requirementId: self.select,
        projectId: self.projectId,
        name: self.name,
        description: self.description,
        responsibles: responsibles,
        stage: self.stage,
      };

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
            content: "Tarefa editado com sucesso!",
            color: "success",
          });
          self.$store.commit("refreshTasks");
          self.close();
        })
        .catch(function(error) {
          console.log(error);
          if (error.response.data.message !== "") {
            self.$snackbar.showMessage({
              content: error.response.data.message,
              color: "error",
            });
          }
        });
    },

    deleteTask() {
      let self = this;
      const URL =
        self.$store.state.apiURL +
        "/projects/" +
        self.projectId +
        "/tasks/" +
        self.taskId;

      const options = {
        headers: {
          Authorization: self.$store.state.user_token,
        },
      };

      axios.delete(URL, options).then(() => {
        self.$snackbar.showMessage({
          content: "Tarefa deletada com sucesso!",
          color: "success",
        });
        self.$store.commit("refreshTasks");
        self.close();
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
