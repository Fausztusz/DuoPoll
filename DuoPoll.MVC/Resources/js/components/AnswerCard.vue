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
        <t-button type="button" class="w-full" @click="save">{{$t("save")}}</t-button>
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
      let body = {
        'Title': this.title,
        'Media': this.media,
        'Url': this.url,
      }
      let submitUrl = "/api/Answer";
      if (this.id) {
        body.Id = this.id;
        submitUrl += `/${this.id}`
      }

      axios({
        method: this.id ? "put" : "post",
        url: submitUrl,
        data: body,
        headers: {"Content-Type": "application/json"}
      })
          .then((res) => {
            this.id = res.data.id
            this.animateBorder(["ring-green-800", "ring-green-700", "ring-green-600", "ring-green-500"], 3)
          })
          .catch((error) => {
            console.log(error);
            this.animateBorder(["ring-red-800", "ring-red-700", "ring-red-600", "ring-red-500"])
          })
          .finally(() => {
            console.log("finally")
          })

    },
    dropzoneMounted() {
      let file = {size: 200, type: "image/png"};
      if (this.answer.Media)
        this.$refs.myVueDropzone.manuallyAddFile(file, this.answer.Media);
    },
    successResponse(file, response) {
      console.log(response);
      console.log(response.url);
      this.media = response.url
    },
    errorResponse(file, message, xhr) {
      this.animateBorder(["ring-red-800", "ring-red-700", "ring-red-600", "ring-red-500"])
    },

    animateBorder(color, speed = 1) {
      this.errorClass = `ring-6 ${color[0]} ring-opacity-100`
      setTimeout(() => {
        this.errorClass = `ring-6 ${color[1]} ring-opacity-100`;
        setTimeout(() => {
          this.errorClass = `ring-8 ${color[2]} ring-opacity-100`;
          setTimeout(() => {
            this.errorClass = `ring-8 ${color[3]} ring-opacity-100`;
            setTimeout(() => {
              this.errorClass = ""
            }, 1000 / speed)
          }, 1000 / speed)
        }, 1000 / speed)
      }, 1000 / speed)
    }
  },
  // watch: {
  //   dropdown: function (val) {
  //     this.dropdown = val
  //   }
  // }
};
</script>
