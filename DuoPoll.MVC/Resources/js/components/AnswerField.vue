<template>
  <div class="answer-wrapper">
    <div class="grid grid-cols-12 gap-4">
      <div v-for="answer in answers" class="col-span-12 md:col-span-6 lg:col-span-3">
        <answer-card :title="answer.title" :media="answer.media"></answer-card>
      </div>
      <div class="col-span-12 md:col-span-3 flex justify-center" style="align-items: center">
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
      answers: [{
        title: "someTitle0",
        media: "https://someimage.blob.core.windows.net/image-container/0c4d6627-864a-4f33-8e1d-e914af19afe1Joe.png"
      }, {
        title: "someTitle2",
        media: "https://someimage.blob.core.windows.net/image-container/6eb70105-1b18-4628-8483-523f2d07f754joker.jpg"
      }, {
        title: "someTitle3",
        media: ""
      }, {
        title: "someTitle4",
        media: "https://someimage.blob.core.windows.net/image-container/9a5f58a1-efa2-4d09-b135-1891b36d8913joker.png"
      }, {
        title: "someTitle5",
        media: "https://someimage.blob.core.windows.net/image-container/0c4d6627-864a-4f33-8e1d-e914af19afe1Joe.png"
      }],
    }
  },
  methods: {
    newCard() {
      this.answers.push({title: ""})
    }
  },
};
</script>
