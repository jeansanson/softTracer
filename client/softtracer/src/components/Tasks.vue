<template>
  <div>
    <v-row>
      <v-col cols="12" md="4">
        <v-text-field
          v-model="newTask"
          required
          label="Tarefa"
          :counter="255"
          @keyup.enter="addTask"
        ></v-text-field>
      </v-col>
      <v-col cols="12" md="4" class="mt-4">
        <v-btn @click="addTask" color="primary" class="ml-3">Adicionar</v-btn>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12" md="3">
        <v-card color="blue-grey lighten-5" height="100%" min-height="687px">
          <v-card-title>Backlog</v-card-title>
          <draggable
            class="list-group kanban-column"
            :list="arrBackLog"
            group="tasks"
            @change="changed(1, $event)"
          >
            <v-card
              v-for="element in arrBackLog"
              :key="element.id"
              class="mb-2 ml-2 mr-2"
              style="cursor: pointer;"
            >
              <v-card-title style="font-size: 16px">{{
                element.name
              }}</v-card-title>
            </v-card>
          </draggable>
        </v-card>
      </v-col>

      <v-col cols="12" md="3">
        <v-card color="blue-grey lighten-5" height="100%" min-height="687px">
          <v-card-title>To-do</v-card-title>
          <draggable
            class="list-group kanban-column"
            :list="arrToDo"
            group="tasks"
            @change="changed(2, $event)"
          >
            <v-card
              v-for="element in arrToDo"
              :key="element.id"
              class="mb-2 ml-2 mr-2"
              style="cursor: pointer;"
            >
              <v-card-title style="font-size: 16px">{{
                element.name
              }}</v-card-title>
            </v-card>
          </draggable>
        </v-card>
      </v-col>

      <v-col cols="12" md="3">
        <v-card color="blue-grey lighten-5" height="100%" min-height="687px">
          <v-card-title>Doing</v-card-title>
          <draggable
            class="list-group kanban-column"
            :list="arrDoing"
            group="tasks"
            @change="changed(3, $event)"
          >
            <v-card
              v-for="element in arrDoing"
              :key="element.id"
              class="mb-2 ml-2 mr-2"
              style="cursor: pointer;"
            >
              <v-card-title style="font-size: 16px">{{
                element.name
              }}</v-card-title>
            </v-card>
          </draggable>
        </v-card>
      </v-col>

      <v-col cols="12" md="3">
        <v-card color="blue-grey lighten-5" height="100%" min-height="687px">
          <v-card-title>Done</v-card-title>
          <draggable
            class="list-group kanban-column"
            :list="arrDone"
            group="tasks"
            @change="changed(4, $event)"
          >
            <v-card
              v-for="element in arrDone"
              :key="element.id"
              class="mb-2 ml-2 mr-2"
              style="cursor: pointer;"
            >
              <v-card-title style="font-size: 16px">{{
                element.name
              }}</v-card-title>
            </v-card>
          </draggable>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script>
import draggable from "vuedraggable";
const axios = require("axios");

export default {
  name: "Tasks",
  components: {
    draggable,
  },
  data() {
    return {
      newTask: "",
      arrBackLog: [],
      arrToDo: [],
      arrDoing: [],
      arrDone: [],
    };
  },
  created: function() {
    this.getTasks();
  },
  methods: {
    //add new tasks method
    addTask() {
      let self = this;

      const URL =
        self.$store.state.apiURL +
        "/projects/" +
        this.$store.state.currentProjectId +
        "/tasks";

      const data = {
        name: this.newTask,
        projectId: this.$store.state.currentProjectId,
        stage: 1,
      };

      const options = {
        headers: {
          Authorization: self.$store.state.user_token,
        },
      };

      axios
        .post(URL, data, options)
        .then(function() {
          self.$snackbar.showMessage({
            content: "Tarefa criada com sucesso!",
            color: "success",
          });
          self.newTask = "";
          self.getTasks();
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

    getTasks() {
      this.arrBackLog = [];
      this.arrToDo = [];
      this.arrDoing = [];
      this.arrDone = [];

      let self = this;
      const URL =
        self.$store.state.apiURL +
        "/projects/" +
        this.$store.state.currentProjectId +
        "/tasks";

      const options = {
        headers: {
          Authorization: self.$store.state.user_token,
        },
      };

      axios.get(URL, options).then((resp) => {
        var backlog = [];
        var todo = [];
        var doing = [];
        var done = [];

        resp.data.forEach((element) => {
          if(element.requirementName && element.requirementName.length > 0) element.name += ` [REQ: ${element.requirementName}]`;
          switch (element["stage"]) {
            case 1:
              backlog.push(element);
              break;
            case 2:
              todo.push(element);
              break;
            case 3:
              doing.push(element);
              break;
            case 4:
              done.push(element);
              break;
            default:
              break;
          }
        });

        self.arrBackLog = backlog;
        self.arrToDo = todo;
        self.arrDoing = doing;
        self.arrDone = done;
      });
    },

    changed(stage, event) {
      // console.log(id, event);
      if ("added" in event) {
        var task = event.added.element;
        this.editTaskStage(task.id, task.name, stage);
      }
    },

    editTaskStage(id, name, stage) {
      let self = this;

      const URL =
        self.$store.state.apiURL +
        "/projects/" +
        this.$store.state.currentProjectId +
        "/tasks/" +
        id;

      const data = {
        id: id,
        name: name,
        projectId: this.$store.state.currentProjectId,
        stage: stage,
      };

      const options = {
        headers: {
          Authorization: self.$store.state.user_token,
        },
      };

      axios
        .put(URL, data, options)
        .then(function() {
          self.getTasks();
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
  },
};
</script>

<style>
.kanban-column {
  min-height: 300px;
}
</style>
