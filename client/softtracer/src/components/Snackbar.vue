<template>
  <v-snackbar v-model="show" :color="color" :timeout="timeout">
    {{ message }}
    <v-btn text @click="show = false" class="text-right">Close</v-btn>
  </v-snackbar>
</template>

<script>
export default {
  data() {
    return {
      show: false,
      message: "",
      color: "",
      timeout: 5000,
    };
  },

  created() {
    this.$store.subscribe((mutation, state) => {
      if (mutation.type === "showMessage") {
        this.message = state.content;
        this.color = state.color;
        this.show = true;
      }
    });
  },
};
</script>
