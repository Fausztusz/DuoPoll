<template>
  <div class="answer-wrapper">
    <div class="grid grid-cols-12 gap-4">
      <div v-for="answer in answers" class="col-span-3">
        <answer-card :title="answer.title"></answer-card>
      </div>
      <div class="col-span-3 flex justify-center" style="align-items: center">
        <t-button @click="newCard" type="button" class="rounded-full max-w-l">+</t-button>
      </div>
    </div>
  </div>
</template>

<script>
import AnswerCard from "./AnswerCard";

export default {
  name: "AnswerField",
  components: {
    answerCard: AnswerCard,
  },
  data() {
    return {
      answers: [{title: "someTitle0"}, {title: "someTitle2"}, {title: "someTitle3"}, {title: "someTitle4"}, {title: "someTitle5"}],
    }
  },
  methods: {
    newCard() {
      this.answers.push({title: ""})
    }
  },
};
</script>
