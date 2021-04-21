<template>
  <div class="answer-card-wrapper h-full">
    <div class="grid grid-cols-1 h-full gap-y-5 p-5 bg-gray-500 border-gray-200 rounded-lg">
      <div class="col-span-1">
        <label>
          <t-input placeholder="Name" :value="title">{{ title }}</t-input>
        </label>
      </div>
      <div class="col-span-1">
        <label>
          <t-select v-model="dropdown"
                    :options="[{value:'image',text:'Upload image'},{value:'video',text:'Youtube link'}]"></t-select>
        </label>
      </div>
      <div class="col-span-1 min-h-42 max-h-52">
        <div v-if="media==='image'">
          <vue-dropzone ref="myVueDropzone" id="dropzone" :options="dropzoneOptions"></vue-dropzone>
        </div>
        <div v-if="media==='video'">
          <t-input placeholder="Youtube link"></t-input>
        </div>
      </div>
      <div class="col-span-1">
        <t-button type="button" class="w-full" @click="save">Save</t-button>
      </div>
    </div>


  </div>
</template>

<script>
import vue2Dropzone from 'vue2-dropzone'
import 'vue2-dropzone/dist/vue2Dropzone.min.css'

export default {
  name: "AnswerCard",
  components: {
    vueDropzone: vue2Dropzone,
  },
  props: {
    title: {
      type: String,
      default: ""
    }
  },
  data() {
    return {
      dropdown: "image",
      media: "image",
      dropzoneOptions: {
        url: 'https://httpbin.org/post',
        thumbnailWidth: 200,
        maxFilesize: 3,
        headers: {"My-Awesome-Header": "header value"}
      }
    }
  },
  methods: {
    save() {
      let bodyFormData = new FormData();
      bodyFormData.append('Title', this.title);
      bodyFormData.append('Media', this.media);
      bodyFormData.append('PollId', 5);

      axios({
        method: "post",
        url: "/api/Answer/",
        data: bodyFormData,
        headers: {"Content-Type": "multipart/form-data"}
      })
          .then((res) => {
          })
          .catch((error) => {
          })
          .finally(() => {
          })

    }
  },
  watch: {
    dropdown: function (val) {
      this.media = val
    }
  }
};
</script>
