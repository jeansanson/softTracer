import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

export default new Vuex.Store({
  state: { content: "", color: "" },
  mutations: {
    showMessage(state, payload) {
      state.content = payload.content;
      state.color = payload.color;
    },
  },
  actions: {},
  modules: {},
});
