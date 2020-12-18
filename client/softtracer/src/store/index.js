import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    snackbar_content: "",
    snackbar_color: "",
    user_name: "",
    user_email: "",
    user_token: "",
    user_id: "",
    apiURL: "",
    currentProjectId: "",
    requirements: "",
    editRequirement: [],
    selectedTask: [],
  },
  mutations: {
    // Show snackbar
    showMessage(state, payload) {
      state.snackbar_content = payload.content;
      state.snackbar_color = payload.color;
    },

    showCreateProject() {},
    
    showEnterProject() {},

    showEditTask(state, payload) {
      state.selectedTask = payload.task;
    },

    setCurrentProjectId(state, id) {
      state.currentProjectId = id;
    },

    setRequirements(state, requirements) {
      state.requirements = requirements;
    },

    showCreateRequirement(state, payload) {
      state.currentProjectId = payload.projectId;
      state.requirements = payload.requirements;
    },

    showEditRequirement(state, payload) {
      state.currentProjectId = payload.projectId;
      state.requirements = payload.requirements;
      state.editRequirement = payload.editRequirement;
    },

    refreshProjects() {},
    
    refreshRequirements() {},

    refreshTasks() {},

    // Login credentials
    storeLogin(state, payload) {
      state.user_name = payload.displayName;
      state.user_email = payload.email;
      state.user_token = payload.token;
      state.user_id = payload.userId;
      localStorage.setItem("user_name", payload.displayName);
      localStorage.setItem("user_email", payload.email);
      localStorage.setItem("user_token", payload.token);
      localStorage.setItem("user_id", payload.userId);
    },

    logout(state) {
      (state.user_name = ""),
        (state.user_email = ""),
        (state.user_token = ""),
        (state.user_id = "");
      localStorage.setItem("user_name", "");
      localStorage.setItem("user_email", "");
      localStorage.setItem("user_token", "");
      localStorage.setItem("user_id", "");
    },

    initialiseStore(state) {
      (state.user_name = localStorage.getItem("user_name")),
        (state.user_email = localStorage.getItem("user_email")),
        (state.user_token = localStorage.getItem("user_token")),
        (state.user_id = localStorage.getItem("user_id")),
        (state.apiURL = "https://localhost:44342/api");
    },
  },
  actions: {},
  modules: {},
});
