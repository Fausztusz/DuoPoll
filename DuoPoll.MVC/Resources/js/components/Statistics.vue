<template>
  <div :class="{pyro:firework}">
    <div :class="{before:firework}"></div>
    <div :class="{after:firework}"></div>
    <div v-if="isLoaded && answers.length" class="hidden sm:grid grid-cols-3 gap-4 justify-around h-44">
      <div
          class="grid grid-cols-4 grid-cols-1 p-3 h-32 border border-purple-700 dark:border-gray-200 rounded-lg self-end">
        <div class="col-span-3 text-3xl self-center">
          {{ answers[2].title }}
        </div>
        <div class="col-span-1">
          <img :src="getImage(answers[2])" @error="setAltImg" alt="Img" class="h-24 object-cover">
        </div>
      </div>
      <div @click="firework=!firework"
          class="grid grid-cols-4 grid-cols-1 p-3 h-32 border border-purple-700 dark:border-gray-200 rounded-lg self-start cursor-pointer">
        <div class="col-span-3 text-3xl self-center">
          {{ answers[0].title }}
        </div>
        <div class="col-span-1">
          <img :src="getImage(answers[0])" @error="setAltImg" alt="Img" class="h-24 object-cover">
        </div>
      </div>
      <div
          class="grid grid-cols-4 grid-cols-1 p-3 h-32 border border-purple-700 dark:border-gray-200 rounded-lg self-center">
        <div class="col-span-3 text-3xl self-center">
          {{ answers[1].title }}
        </div>
        <div class="col-span-1">
          <img :src="getImage(answers[1])" @error="setAltImg" alt="Img" class="h-24 object-cover">
        </div>
      </div>
    </div>

    <hr class="mt-8 mb-4 border-purple-700 dark:border-gray-200">
    <div class="grid grid-cols-2 gap-4 justify-around">
      <div v-for="(answer,index) in answers"
           class="col-span-2 row-span-3 md:col-span-1 p-4 grid grid-cols-6
            dark:bg-gray-600 dark:hover:bg-gray-700 hover:bg-indigo-200 bg-indigo-100
            rounded-lg drop-shadow-2xl drop-shadow-lg hover:drop-shadow-2xl">
        <div class="col-span-2 row-span-3 text-xl self-center">
          <span class="text-2xl">{{ `#${index + 1}` }}</span> {{ answer.title }}
        </div>
        <div class="col-span-1 text-center">
          Win%
        </div>
        <div class="col-span-1 text-left">
          {{ answer.winPercentage }}%
        </div>
        <div class="col-span-2 row-span-3">
          <img :src="getImage(answer)" @error="setAltImg" alt="Img" class="mx-auto h-36 object-cover">
        </div>
        <div class="col-span-1 text-center">
          Wins
        </div>
        <div class="col-span-1 text-left">
          {{ answer.wins }}
        </div>
        <div class="col-span-1 text-center">
          Losses
        </div>
        <div class="col-span-1 text-left">
          {{ answer.loses }}
        </div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "Statistics",
  props: ["url"],
  data() {
    return {
      answers: [],
      title: "",
      isLoaded: false,
      firework:false
    }
  },
  methods: {
    LoadStats() {
      axios({
        method: "GET",
        url: `/api/Statistics/${this.url}`,
      }).then((res) => {
        this.title = res.data.name;
        this.answers = res.data.answers;
        console.log(this.answers);
      }).catch((error) => {
        console.log(error)
      }).finally(() => {
        this.isLoaded = true;
      })
    },
    getImage(answer) {
      return answer.media ?? "/images/placeholder.png";
    },
    setAltImg(event) {
      event.target.src = "/images/placeholder.png"
    },
  },

  mounted() {
    this.LoadStats();
  }
};
</script>

<!--TODO Use SCSS instead of raw css https://codepen.io/yshlin/pen/ylDEk-->
<style>
.pyro > .before, .pyro > .after {
  position: absolute;
  width: 5px;
  height: 5px;
  border-radius: 50%;
  box-shadow: 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff, 0 0 #fff;
  -moz-animation: 1s bang ease-out infinite backwards, 1s gravity ease-in infinite backwards, 5s position linear infinite backwards;
  -webkit-animation: 1s bang ease-out infinite backwards, 1s gravity ease-in infinite backwards, 5s position linear infinite backwards;
  -o-animation: 1s bang ease-out infinite backwards, 1s gravity ease-in infinite backwards, 5s position linear infinite backwards;
  -ms-animation: 1s bang ease-out infinite backwards, 1s gravity ease-in infinite backwards, 5s position linear infinite backwards;
  animation: 1s bang ease-out infinite backwards, 1s gravity ease-in infinite backwards, 5s position linear infinite backwards;
}
.pyro > .after {
  -moz-animation-delay: 1.25s, 1.25s, 1.25s;
  -webkit-animation-delay: 1.25s, 1.25s, 1.25s;
  -o-animation-delay: 1.25s, 1.25s, 1.25s;
  -ms-animation-delay: 1.25s, 1.25s, 1.25s;
  animation-delay: 1.25s, 1.25s, 1.25s;
  -moz-animation-duration: 1.25s, 1.25s, 6.25s;
  -webkit-animation-duration: 1.25s, 1.25s, 6.25s;
  -o-animation-duration: 1.25s, 1.25s, 6.25s;
  -ms-animation-duration: 1.25s, 1.25s, 6.25s;
  animation-duration: 1.25s, 1.25s, 6.25s;
}
@-webkit-keyframes bang {
  to {
    box-shadow: -7px -148.6666666667px #0f5, 47px -96.6666666667px #00ffd0, 136px -396.6666666667px #00ff8c, 138px -326.6666666667px #ff0037, 11px -258.6666666667px #bf0, -75px -385.6666666667px #00ffe6, -170px -300.6666666667px #b3ff00, 183px -334.6666666667px #f30, -47px 55.3333333333px #ff0040, -56px -382.6666666667px #ffae00, -53px 16.3333333333px #a6ff00, -212px 75.3333333333px #c400ff, -245px -381.6666666667px #0037ff, 4px -184.6666666667px #ff00d5, -127px -371.6666666667px #d900ff, -126px 50.3333333333px #09f, 210px -382.6666666667px #ff002f, -159px -176.6666666667px #00ffd9, -247px -268.6666666667px #c4ff00, 146px -94.6666666667px #d9ff00, 2px 60.3333333333px #f90, 126px -54.6666666667px #00ff91, -105px 33.3333333333px #ff001a, 105px -216.6666666667px #ffa600, 178px -83.6666666667px #d900ff, 150px -86.6666666667px #fffb00, -55px -121.6666666667px #0f9, -165px -29.6666666667px #f0e, 34px -288.6666666667px #0f7, -170px 1.3333333333px #fd0, -150px -219.6666666667px #0015ff, 224px -101.6666666667px #f50, -58px -21.6666666667px #ff6200, -139px -385.6666666667px #2600ff, 32px -378.6666666667px #9d00ff, 199px -348.6666666667px #ff0062, 117px -116.6666666667px #2f0, 122px -401.6666666667px #6aff00, -236px -244.6666666667px #ff00bf, -234px -104.6666666667px #ffc800, -143px -259.6666666667px #20f, -52px -47.6666666667px #4cff00, -103px -35.6666666667px #00ff7b, 161px -209.6666666667px #00ff91, -100px -367.6666666667px #00fffb, 197px -228.6666666667px #00ff73, 101px -12.6666666667px #f03, -38px 63.3333333333px #f20, -133px -284.6666666667px #ffae00, -203px -275.6666666667px #0004ff, 124px 2.3333333333px #a600ff;
  }
}
@-moz-keyframes bang {
  to {
    box-shadow: -7px -148.6666666667px #0f5, 47px -96.6666666667px #00ffd0, 136px -396.6666666667px #00ff8c, 138px -326.6666666667px #ff0037, 11px -258.6666666667px #bf0, -75px -385.6666666667px #00ffe6, -170px -300.6666666667px #b3ff00, 183px -334.6666666667px #f30, -47px 55.3333333333px #ff0040, -56px -382.6666666667px #ffae00, -53px 16.3333333333px #a6ff00, -212px 75.3333333333px #c400ff, -245px -381.6666666667px #0037ff, 4px -184.6666666667px #ff00d5, -127px -371.6666666667px #d900ff, -126px 50.3333333333px #09f, 210px -382.6666666667px #ff002f, -159px -176.6666666667px #00ffd9, -247px -268.6666666667px #c4ff00, 146px -94.6666666667px #d9ff00, 2px 60.3333333333px #f90, 126px -54.6666666667px #00ff91, -105px 33.3333333333px #ff001a, 105px -216.6666666667px #ffa600, 178px -83.6666666667px #d900ff, 150px -86.6666666667px #fffb00, -55px -121.6666666667px #0f9, -165px -29.6666666667px #f0e, 34px -288.6666666667px #0f7, -170px 1.3333333333px #fd0, -150px -219.6666666667px #0015ff, 224px -101.6666666667px #f50, -58px -21.6666666667px #ff6200, -139px -385.6666666667px #2600ff, 32px -378.6666666667px #9d00ff, 199px -348.6666666667px #ff0062, 117px -116.6666666667px #2f0, 122px -401.6666666667px #6aff00, -236px -244.6666666667px #ff00bf, -234px -104.6666666667px #ffc800, -143px -259.6666666667px #20f, -52px -47.6666666667px #4cff00, -103px -35.6666666667px #00ff7b, 161px -209.6666666667px #00ff91, -100px -367.6666666667px #00fffb, 197px -228.6666666667px #00ff73, 101px -12.6666666667px #f03, -38px 63.3333333333px #f20, -133px -284.6666666667px #ffae00, -203px -275.6666666667px #0004ff, 124px 2.3333333333px #a600ff;
  }
}
@-o-keyframes bang {
  to {
    box-shadow: -7px -148.6666666667px #0f5, 47px -96.6666666667px #00ffd0, 136px -396.6666666667px #00ff8c, 138px -326.6666666667px #ff0037, 11px -258.6666666667px #bf0, -75px -385.6666666667px #00ffe6, -170px -300.6666666667px #b3ff00, 183px -334.6666666667px #f30, -47px 55.3333333333px #ff0040, -56px -382.6666666667px #ffae00, -53px 16.3333333333px #a6ff00, -212px 75.3333333333px #c400ff, -245px -381.6666666667px #0037ff, 4px -184.6666666667px #ff00d5, -127px -371.6666666667px #d900ff, -126px 50.3333333333px #09f, 210px -382.6666666667px #ff002f, -159px -176.6666666667px #00ffd9, -247px -268.6666666667px #c4ff00, 146px -94.6666666667px #d9ff00, 2px 60.3333333333px #f90, 126px -54.6666666667px #00ff91, -105px 33.3333333333px #ff001a, 105px -216.6666666667px #ffa600, 178px -83.6666666667px #d900ff, 150px -86.6666666667px #fffb00, -55px -121.6666666667px #0f9, -165px -29.6666666667px #f0e, 34px -288.6666666667px #0f7, -170px 1.3333333333px #fd0, -150px -219.6666666667px #0015ff, 224px -101.6666666667px #f50, -58px -21.6666666667px #ff6200, -139px -385.6666666667px #2600ff, 32px -378.6666666667px #9d00ff, 199px -348.6666666667px #ff0062, 117px -116.6666666667px #2f0, 122px -401.6666666667px #6aff00, -236px -244.6666666667px #ff00bf, -234px -104.6666666667px #ffc800, -143px -259.6666666667px #20f, -52px -47.6666666667px #4cff00, -103px -35.6666666667px #00ff7b, 161px -209.6666666667px #00ff91, -100px -367.6666666667px #00fffb, 197px -228.6666666667px #00ff73, 101px -12.6666666667px #f03, -38px 63.3333333333px #f20, -133px -284.6666666667px #ffae00, -203px -275.6666666667px #0004ff, 124px 2.3333333333px #a600ff;
  }
}
@-ms-keyframes bang {
  to {
    box-shadow:  -7px -148.6666666667px #0f5, 47px -96.6666666667px #00ffd0, 136px -396.6666666667px #00ff8c, 138px -326.6666666667px #ff0037, 11px -258.6666666667px #bf0, -75px -385.6666666667px #00ffe6, -170px -300.6666666667px #b3ff00, 183px -334.6666666667px #f30, -47px 55.3333333333px #ff0040, -56px -382.6666666667px #ffae00, -53px 16.3333333333px #a6ff00, -212px 75.3333333333px #c400ff, -245px -381.6666666667px #0037ff, 4px -184.6666666667px #ff00d5, -127px -371.6666666667px #d900ff, -126px 50.3333333333px #09f, 210px -382.6666666667px #ff002f, -159px -176.6666666667px #00ffd9, -247px -268.6666666667px #c4ff00, 146px -94.6666666667px #d9ff00, 2px 60.3333333333px #f90, 126px -54.6666666667px #00ff91, -105px 33.3333333333px #ff001a, 105px -216.6666666667px #ffa600, 178px -83.6666666667px #d900ff, 150px -86.6666666667px #fffb00, -55px -121.6666666667px #0f9, -165px -29.6666666667px #f0e, 34px -288.6666666667px #0f7, -170px 1.3333333333px #fd0, -150px -219.6666666667px #0015ff, 224px -101.6666666667px #f50, -58px -21.6666666667px #ff6200, -139px -385.6666666667px #2600ff, 32px -378.6666666667px #9d00ff, 199px -348.6666666667px #ff0062, 117px -116.6666666667px #2f0, 122px -401.6666666667px #6aff00, -236px -244.6666666667px #ff00bf, -234px -104.6666666667px #ffc800, -143px -259.6666666667px #20f, -52px -47.6666666667px #4cff00, -103px -35.6666666667px #00ff7b, 161px -209.6666666667px #00ff91, -100px -367.6666666667px #00fffb, 197px -228.6666666667px #00ff73, 101px -12.6666666667px #f03, -38px 63.3333333333px #f20, -133px -284.6666666667px #ffae00, -203px -275.6666666667px #0004ff, 124px 2.3333333333px #a600ff;
  }
}
@keyframes bang {
  to {
    box-shadow: -7px -148.6666666667px #0f5, 47px -96.6666666667px #00ffd0, 136px -396.6666666667px #00ff8c, 138px -326.6666666667px #ff0037, 11px -258.6666666667px #bf0, -75px -385.6666666667px #00ffe6, -170px -300.6666666667px #b3ff00, 183px -334.6666666667px #f30, -47px 55.3333333333px #ff0040, -56px -382.6666666667px #ffae00, -53px 16.3333333333px #a6ff00, -212px 75.3333333333px #c400ff, -245px -381.6666666667px #0037ff, 4px -184.6666666667px #ff00d5, -127px -371.6666666667px #d900ff, -126px 50.3333333333px #09f, 210px -382.6666666667px #ff002f, -159px -176.6666666667px #00ffd9, -247px -268.6666666667px #c4ff00, 146px -94.6666666667px #d9ff00, 2px 60.3333333333px #f90, 126px -54.6666666667px #00ff91, -105px 33.3333333333px #ff001a, 105px -216.6666666667px #ffa600, 178px -83.6666666667px #d900ff, 150px -86.6666666667px #fffb00, -55px -121.6666666667px #0f9, -165px -29.6666666667px #f0e, 34px -288.6666666667px #0f7, -170px 1.3333333333px #fd0, -150px -219.6666666667px #0015ff, 224px -101.6666666667px #f50, -58px -21.6666666667px #ff6200, -139px -385.6666666667px #2600ff, 32px -378.6666666667px #9d00ff, 199px -348.6666666667px #ff0062, 117px -116.6666666667px #2f0, 122px -401.6666666667px #6aff00, -236px -244.6666666667px #ff00bf, -234px -104.6666666667px #ffc800, -143px -259.6666666667px #20f, -52px -47.6666666667px #4cff00, -103px -35.6666666667px #00ff7b, 161px -209.6666666667px #00ff91, -100px -367.6666666667px #00fffb, 197px -228.6666666667px #00ff73, 101px -12.6666666667px #f03, -38px 63.3333333333px #f20, -133px -284.6666666667px #ffae00, -203px -275.6666666667px #0004ff, 124px 2.3333333333px #a600ff;
  }
}
@-webkit-keyframes gravity {
  to {
    transform: translateY(200px);
    -moz-transform: translateY(200px);
    -webkit-transform: translateY(200px);
    -o-transform: translateY(200px);
    -ms-transform: translateY(200px);
    opacity: 0;
  }
}
@-moz-keyframes gravity {
  to {
    transform: translateY(200px);
    -moz-transform: translateY(200px);
    -webkit-transform: translateY(200px);
    -o-transform: translateY(200px);
    -ms-transform: translateY(200px);
    opacity: 0;
  }
}
@-o-keyframes gravity {
  to {
    transform: translateY(200px);
    -moz-transform: translateY(200px);
    -webkit-transform: translateY(200px);
    -o-transform: translateY(200px);
    -ms-transform: translateY(200px);
    opacity: 0;
  }
}
@-ms-keyframes gravity {
  to {
    transform: translateY(200px);
    -moz-transform: translateY(200px);
    -webkit-transform: translateY(200px);
    -o-transform: translateY(200px);
    -ms-transform: translateY(200px);
    opacity: 0;
  }
}
@keyframes gravity {
  to {
    transform: translateY(200px);
    -moz-transform: translateY(200px);
    -webkit-transform: translateY(200px);
    -o-transform: translateY(200px);
    -ms-transform: translateY(200px);
    opacity: 0;
  }
}
@-webkit-keyframes position {
  0%, 19.9% {
    margin-top: 10%;
    margin-left: 40%;
  }
  20%, 39.9% {
    margin-top: 40%;
    margin-left: 30%;
  }
  40%, 59.9% {
    margin-top: 20%;
    margin-left: 70%;
  }
  60%, 79.9% {
    margin-top: 30%;
    margin-left: 20%;
  }
  80%, 99.9% {
    margin-top: 30%;
    margin-left: 80%;
  }
}
@-moz-keyframes position {
  0%, 19.9% {
    margin-top: 10%;
    margin-left: 40%;
  }
  20%, 39.9% {
    margin-top: 40%;
    margin-left: 30%;
  }
  40%, 59.9% {
    margin-top: 20%;
    margin-left: 70%;
  }
  60%, 79.9% {
    margin-top: 30%;
    margin-left: 20%;
  }
  80%, 99.9% {
    margin-top: 30%;
    margin-left: 80%;
  }
}
@-o-keyframes position {
  0%, 19.9% {
    margin-top: 10%;
    margin-left: 40%;
  }
  20%, 39.9% {
    margin-top: 40%;
    margin-left: 30%;
  }
  40%, 59.9% {
    margin-top: 20%;
    margin-left: 70%;
  }
  60%, 79.9% {
    margin-top: 30%;
    margin-left: 20%;
  }
  80%, 99.9% {
    margin-top: 30%;
    margin-left: 80%;
  }
}
@-ms-keyframes position {
  0%, 19.9% {
    margin-top: 10%;
    margin-left: 40%;
  }
  20%, 39.9% {
    margin-top: 40%;
    margin-left: 30%;
  }
  40%, 59.9% {
    margin-top: 20%;
    margin-left: 70%;
  }
  60%, 79.9% {
    margin-top: 30%;
    margin-left: 20%;
  }
  80%, 99.9% {
    margin-top: 30%;
    margin-left: 80%;
  }
}
@keyframes position {
  0%, 19.9% {
    margin-top: 10%;
    margin-left: 40%;
  }
  20%, 39.9% {
    margin-top: 40%;
    margin-left: 30%;
  }
  40%, 59.9% {
    margin-top: 20%;
    margin-left: 70%;
  }
  60%, 79.9% {
    margin-top: 30%;
    margin-left: 20%;
  }
  80%, 99.9% {
    margin-top: 30%;
    margin-left: 80%;
  }
}

</style>
