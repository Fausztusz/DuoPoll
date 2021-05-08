<template>
  <div class="answer-card-wrapper h-full">
    <button @click="deleteAnswer" :class="`float relative bg-blue-900 rounded-full p-2 focus:ring-4 ${delError}`"
            style="top:-17px;left:-22px">
      <svg height="18pt" viewBox="-40 0 427 427.00131" width="18pt" style="fill: white"
           xmlns="http://www.w3.org/2000/svg">
        <path
            d="m232.398438 154.703125c-5.523438 0-10 4.476563-10 10v189c0 5.519531 4.476562 10 10 10 5.523437 0 10-4.480469 10-10v-189c0-5.523437-4.476563-10-10-10zm0 0"/>
        <path
            d="m114.398438 154.703125c-5.523438 0-10 4.476563-10 10v189c0 5.519531 4.476562 10 10 10 5.523437 0 10-4.480469 10-10v-189c0-5.523437-4.476563-10-10-10zm0 0"/>
        <path
            d="m28.398438 127.121094v246.378906c0 14.5625 5.339843 28.238281 14.667968 38.050781 9.285156 9.839844 22.207032 15.425781 35.730469 15.449219h189.203125c13.527344-.023438 26.449219-5.609375 35.730469-15.449219 9.328125-9.8125 14.667969-23.488281 14.667969-38.050781v-246.378906c18.542968-4.921875 30.558593-22.835938 28.078124-41.863282-2.484374-19.023437-18.691406-33.253906-37.878906-33.257812h-51.199218v-12.5c.058593-10.511719-4.097657-20.605469-11.539063-28.03125-7.441406-7.421875-17.550781-11.5546875-28.0625-11.46875h-88.796875c-10.511719-.0859375-20.621094 4.046875-28.0625 11.46875-7.441406 7.425781-11.597656 17.519531-11.539062 28.03125v12.5h-51.199219c-19.1875.003906-35.394531 14.234375-37.878907 33.257812-2.480468 19.027344 9.535157 36.941407 28.078126 41.863282zm239.601562 279.878906h-189.203125c-17.097656 0-30.398437-14.6875-30.398437-33.5v-245.5h250v245.5c0 18.8125-13.300782 33.5-30.398438 33.5zm-158.601562-367.5c-.066407-5.207031 1.980468-10.21875 5.675781-13.894531 3.691406-3.675781 8.714843-5.695313 13.925781-5.605469h88.796875c5.210937-.089844 10.234375 1.929688 13.925781 5.605469 3.695313 3.671875 5.742188 8.6875 5.675782 13.894531v12.5h-128zm-71.199219 32.5h270.398437c9.941406 0 18 8.058594 18 18s-8.058594 18-18 18h-270.398437c-9.941407 0-18-8.058594-18-18s8.058593-18 18-18zm0 0"/>
        <path
            d="m173.398438 154.703125c-5.523438 0-10 4.476563-10 10v189c0 5.519531 4.476562 10 10 10 5.523437 0 10-4.480469 10-10v-189c0-5.523437-4.476563-10-10-10zm0 0"/>
      </svg>
    </button>
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
        <t-button type="button" class="w-full" @click="save">{{ $t("save") }}</t-button>
      </div>
    </div>
  </div>
</template>

<style>
.float {
  float: right;
}
</style>

<script>
import vue2Dropzone from 'vue2-dropzone'
import 'vue2-dropzone/dist/vue2Dropzone.min.css'
import {bus} from './AnswerField';

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
      delError: "",
      title: "",
      id: null,
      recentlyCreated: false,

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

    if (this.id == null)
      this.recentlyCreated = true;
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
    },
    deleteAnswer() {
      if (this.id === null)
        return bus.$emit('delete-answer', {id: null, hash: this.answer.Hash})

      axios({
        method: "DELETE",
        url: `/api/Answer/${this.id}`,
        data: {},
        headers: {"Content-Type": "application/json"}
      }).then((res) => {
        bus.$emit('delete-answer', {id: this.recentlyCreated ? null : this.id, hash: this.answer.Hash})
      }).catch((error) => {
        this.delError = "ring-8 ring-red-900"
      }).finally(() => {
      })
    }
  },
  // watch: {
  //   dropdown: function (val) {
  //     this.dropdown = val
  //   }
  // }
};
</script>
