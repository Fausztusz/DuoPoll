<template>
  <div class="answer-card-wrapper h-full">
    <div
        :class="errorClass+'transition duration-1000 grid grid-cols-1 h-full gap-y-5 p-5 bg-gray-500 border-gray-200 rounded-lg'">
      <div class="col-span-1">
        <label>
          <t-input placeholder="Name" v-model="title">{{ title }}</t-input>
        </label>
      </div>
      <div class="col-span-1">
        <!--        <label>-->
        <!--          <t-select v-model="dropdown"-->
        <!--                    :options="[{value:'image',text:'Upload image'},{value:'video',text:'Youtube link'}]"></t-select>-->
        <!--        </label>-->
      </div>
      <div class="col-span-1">
        <div v-if="dropdown==='image'" style="height: 240px">
          <vue-dropzone ref="myVueDropzone" id="dropzone" style="display: flex; justify-content: center; height: 240px"
                        @vdropzone-success="successResponse" @vdropzone-error="errorResponse"
                        @vdropzone-mounted="dropzoneMounted"
                        :options="dropzoneOptions">
          </vue-dropzone>
        </div>
        <!--        <div v-if="dropdown==='video'">-->
        <!--          <t-input placeholder="Youtube link"></t-input>-->
        <!--        </div>-->
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
    answer: {
      type: Object,
    },
    url: {
      type: String,
      required: true
    },
  },
  data() {
    return {
      media: "",
      title: "",
      id: null,

      dropdown: "image",
      errorClass: "",

      dropzoneOptions: {
        url: '/Image/Upload',
        thumbnailWidth: 200,
        thumbnailHeight: 200,
        thumbnailMethod: "crop",
        addRemoveLinks: true,
        maxFiles: 1,
        maxFilesize: 3,
        headers: {"My-Awesome-Header": "header value"}
      }
    }
  },
  mounted() {
    this.media = this.answer.Media
    this.title = this.answer.Title
    this.id = this.answer.Id
  },

  methods: {
    save() {
      let bodyFormData = new FormData();
      bodyFormData.append('Title', this.title);
      bodyFormData.append('Media', this.media);
      bodyFormData.append('PollId', 5);

      axios({
        method: this.id ? "put" : "post",
        url: submitUrl,
        data: body,
        headers: {"Content-Type": this.id ? "application/json" : "multipart/form-data"}
      })
          .then((res) => {
          })
          .catch((error) => {
          })
          .finally(() => {
          })

    },
    dropzoneMounted() {
      let file = {size: 200, type: "image/png"};
      if (this.answer.Media)
        this.$refs.myVueDropzone.manuallyAddFile(file, this.answer.Media);
    },
    successResponse(file, response) {
      console.log(response);
      this.media = response.url
    },
    errorResponse(file, message, xhr) {
      this.errorClass = "ring-6 ring-red-800 ring-opacity-100"
      setTimeout(() => {
        this.errorClass = "ring-6 ring-red-700 ring-opacity-100";
        setTimeout(() => {
          this.errorClass = "ring-8 ring-red-600 ring-opacity-100";
          setTimeout(() => {
            this.errorClass = "ring-8 ring-red-500 ring-opacity-100";
            setTimeout(() => {
              this.errorClass = ""
            }, 1000)
          }, 1000)
        }, 1000)
      }, 1000)
    },
  },
  // watch: {
  //   dropdown: function (val) {
  //     this.dropdown = val
  //   }
  // }
};
</script>
