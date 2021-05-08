<template>

  <div class="grid grid-cols-2 text-center align-baseline gap-5 md:gap-10 max-w-sm md:max-w-4xl mx-auto">
    <div v-if="errorMessage" class="col-span-2 text-3xl text-red-900">{{ errorMessage }}</div>
    <a class="col-span-1" href="/Poll">
      <t-button class="mx-auto" v-if="newGame">{{ $t("new_game")}}</t-button>
    </a>
    <a class="col-span-1" :href="`/Poll/Statistics/${url}`">
      <t-button class="mx-auto" v-if="newGame">{{ $t("show_stats") }}</t-button>
    </a>

    <div class="col-span-2 md:col-span-1 h-3xl bg-gray-300 dark:bg-gray-600 rounded-lg">
      <loader v-if="!isLoaded"></loader>
      <div v-if="isLoaded && !newGame">
        <div class="grid grid-cols-1 font-display items-end m-3 md:m-8 gap-y-3 md:gap-y-20">
          <div class="items">
            <img class="md:min-w-full object-cover max-h-36 md:max-h-72 mx-auto overflow-hidden" alt="Image"
                 :src="answers.left.media" @error="setAltImg">
          </div>
          <div class="text-4xl">{{ answers.left.title }}</div>
          <div>
            <button @click="vote('left')"
                    class="bg-purple-600 text-gray-200 dark:bg-gray-500 rounded-full text-2xl py-2 px-4 md:px-8 md:py-4 ">
              {{ $t("select") }}
            </button>
          </div>
        </div>
      </div>
    </div>

    <div class="col-span-2 md:col-span-1 h-3xl bg-gray-300 dark:bg-gray-600 rounded-lg">
      <loader v-if="!isLoaded"></loader>
      <div v-if="isLoaded && !newGame">
        <div class="grid grid-cols-1 font-display items-end m-3 md:m-8 gap-y-3 md:gap-y-20">
          <div>
            <img class="md:min-w-full object-cover max-h-36 md:max-h-72 mx-auto overflow-hidden" alt="Image"
                 :src="answers.right.media" @error="setAltImg">
          </div>
          <div class="text-4xl">{{ answers.right.title }}</div>
          <div>
            <button @click="vote('right')"
                    class="bg-purple-600 text-gray-200 dark:bg-gray-500 rounded-full text-2xl py-2 px-4 md:px-8 md:py-4 ">
              {{ $t("select") }}
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>


</template>
<script>
export default {
  name: "Vote",
  props: ["url"],
  data() {
    return {
      answers: {},
      isLoaded: false,
      newGame: false,
      errorMessage: null,
    }
  },
  methods: {
    setAltImg(event) {
      event.target.src = "/images/placeholder.png"
    },
    vote(side) {
      if (side === "left" || side === "right") {
        axios({
          method: "POST",
          url: `/api/Answer/${this.url}/Vote`,
          data: {Side: side}
        }).then((res) => {
        }).catch((error) => {
        }).finally(() => this.loadQuestion());
      }
    },

    loadQuestion() {
      axios({
        method: "GET",
        url: "/api/Answer/" + this.url,
      }).then((res) => {
        this.answers = {
          left: res.data.left,
          right: res.data.right,
        };
      }).catch((error) => {
        this.errorMessage = error.response.data
        if ([404, 422, 500].includes(error.response.status)) {
          this.newGame = true;
        }
      }).finally(() => {
        this.isLoaded = true;
      })
    }
  },

  mounted() {
    this.loadQuestion();
  }
};
</script>
