import Vue from "vue";
// import "tailwindcss/tailwind.css"

window.Vue = require("vue");

Vue.config.devtools = process.env.NODE_ENV === 'development';
Vue.config.productionTip = process.env.NODE_ENV === 'development';

var app = new Vue({
    el: '#app',
    data: {
        lastname: '',
        firstname: '',
    }
});
