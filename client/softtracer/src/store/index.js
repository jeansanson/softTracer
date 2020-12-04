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
  },
  mutations: {
    // Show snackbar
    showMessage(state, payload) {
      state.snackbar_content = payload.content;
      state.snackbar_color = payload.color;
    },

    showCreateProject() {},

    refreshProjects() {},

    // Login credentials
    storeLogin(state, payload) {
      state.user_name = payload.DisplayName;
      state.user_email = payload.Email;
      state.user_token = payload.Token;
      state.user_id = payload.UserId;
      localStorage.setItem("user_name", payload.DisplayName);
      localStorage.setItem("user_email", payload.Email);
      localStorage.setItem("user_token", payload.Token);
      localStorage.setItem("user_id", payload.UserId);
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
