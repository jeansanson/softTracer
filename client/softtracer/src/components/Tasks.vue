<template>
  <div>
    <v-row>
      <v-col cols="12" md="4">
        <v-text-field
          v-model="newTask"
          required
          label="Tarefa"
          @keyup.enter="add"
        ></v-text-field>
      </v-col>
      <v-col cols="12" md="4" class="mt-4">
        <v-btn @click="add" color="primary" class="ml-3">Adicionar</v-btn>
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
          >
            <v-card
              v-for="element in arrBackLog"
              :key="element.name"
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
          >
            <v-card
              v-for="element in arrToDo"
              :key="element.name"
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
          >
            <v-card
              v-for="element in arrDoing"
              :key="element.name"
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
          >
            <v-card
              v-for="element in arrDone"
              :key="element.name"
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
//import draggable
import draggable from "vuedraggable";

export default {
  name: "Tasks",
  components: {
    draggable,
  },
  data() {
    return {
      newTask: "",
      arrBackLog: [
        { name: "Task 1" },
        { name: "Task 2" },
        { name: "Task 3" },
        { name: "Task 4" },
      ],
      arrToDo: [],
      arrDoing: [],
      arrDone: [],
    };
  },
  methods: {
    //add new tasks method
    add: function() {
      if (this.newTask) {
        this.arrBackLog.push({ name: this.newTask });
        this.newTask = "";
      }
    },
  },
};
</script>

<style>
/* light stylings for the kanban columns */
.kanban-column {
  min-height: 300px;
}
</style>
