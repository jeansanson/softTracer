import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

export default new Vuex.Store({
  state: { snackbar_content: "", snackbar_color: "", name: "", email: "", user_token: "",  user_id: ""},
  mutations: {
    // Show snackbar
    showMessage(state, payload) {
      state.snackbar_content = payload.content;
      state.snackbar_color = payload.color;
    },

    showCreateProject(){

    },

    // Login credentials
    storeLogin(state, payload) {
      state.name = payload.DisplayName;
      state.email = payload.Email;
      state.user_token = payload.Token;
      state.user_id = payload.UserId;
    },

    logout(state) {
      state.name = "",
      state.email = "",
      state.user_token = "",
      state.user_id = ""
    }
  },
  actions: {},
  modules: {},
});
