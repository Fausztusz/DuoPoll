<template>
  <div class="answer-wrapper mt-16">
    <div class="grid grid-cols-12 gap-y-8 ">
      <div v-for="answer in answersList" class="col-span-12 md:col-span-6 lg:col-span-3">
        <answer-card :answer="answer" :url="url"></answer-card>
      </div>
      <div class="col-span-12 md:col-span-3 flex justify-center" style="align-items: center">
        <t-button @click="newCard" type="button" class="rounded-full max-w-l">+</t-button>
      </div>
    </div>
  </div>
</template>

<script>

import Vue from "vue";
import AnswerCard from "./AnswerCard";

export const bus = new Vue();

export default {
  name: "AnswerField",
  components: {
    answerCard: AnswerCard,
  },
  props: {
    url: {
      type: String,
      required: true
    },
    answers: {
      type: Array | null,
      required: true
    },
  },
  data() {
    return {
      answersList: []
    }
  },
  mounted() {
    if (this.answers)
      this.answersList = this.answers;
  },
  methods: {
    newCard() {
      this.answersList.push({Title: "", Media: "", Id: null, Hash: Math.floor(Math.random() * 10e10)})
    }
  },
  created() {
    bus.$on('delete-answer', (data) => {
      this.answersList = this.answersList.filter((el) => {
        if (data.id)
          return el.Id !== data.id
        return el.Hash !== data.hash
      })
    })
  }

};
</script>
