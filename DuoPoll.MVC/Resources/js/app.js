import Vue from "vue";
import VueAxios from 'vue-axios'
import axios from 'axios'
import VueTailwind from 'vue-tailwind'

import TInput from "vue-tailwind/dist/t-input";
import TCheckbox from "vue-tailwind/dist/t-checkbox";
import TDatepicker from "vue-tailwind/dist/t-datepicker";
import TButton from "vue-tailwind/dist/t-button";
import TSelect from "vue-tailwind/dist/t-select";

import Hungarian from "vue-tailwind/dist/l10n/hu";
import English from "vue-tailwind/dist/l10n/default";

import AnswerField from "./components/AnswerField";
import Vote from "./components/Vote";
import Statistics from "./components/Statistics";
import Loader from "./components/Loader";

window.Vue = require("vue");
window.axios = axios;
window.VueAxios = VueAxios;

const settings = {
    'date-picker': {
        component: TDatepicker,
        props: {
            locales: {en: English, hu: Hungarian},
            lang: 'hu',
            timepicker: true,
            inline: false,
            weekStart: 1,
            dateFormat: 'Y-m-d H:m',
            userFormat: 'Y-m-d H:m',
            minDate: new Date(new Date().setHours(0, 0, 0, 0)),
            initialTime: new Date().getHours().toString(),
            fixedClasses: {
                navigator: 'flex',
                navigatorViewButton: 'flex items-center',
                navigatorViewButtonIcon: 'flex-shrink-0 h-5 w-5',
                navigatorViewButtonBackIcon: 'flex-shrink-0 h-5 w-5',
                navigatorLabel: 'flex items-center py-1',
                navigatorPrevButtonIcon: 'h-6 w-6 inline-flex',
                navigatorNextButtonIcon: 'h-6 w-6 inline-flex',
                inputWrapper: 'relative',
                viewGroup: 'inline-flex flex-wrap',
                view: 'w-64',
                calendarDaysWrapper: 'grid grid-cols-7',
                calendarHeaderWrapper: 'grid grid-cols-7',
                monthWrapper: 'grid grid-cols-4',
                yearWrapper: 'grid grid-cols-4',
                input: 'block w-full px-3 py-2 transition duration-100 ease-in-out border rounded shadow-sm focus:border-blue-500 focus:ring-2 focus:ring-blue-500 focus:outline-none focus:ring-opacity-50 disabled:opacity-50 disabled:cursor-not-allowed',
                clearButton: 'flex flex-shrink-0 items-center justify-center absolute right-0 top-0 m-2 h-6 w-6',
                clearButtonIcon: 'fill-current h-3 w-3',

                dropdown: 'dark:bg-gray-700',
                timepickerTimeFieldsWrapper: 'dark:bg-gray-700',
                timepickerOkButton: 'dark:text-gray-200',
                day: 'dark:hover:text-gray-900',
                today: 'dark:hover:text-gray-900',
            },
        }
    },
    't-input': {component: TInput},
    'checkbox': {component: TCheckbox},
    't-select': {component: TSelect},
    't-button': {
        component: TButton,
        props: {
            fixedClasses: 'block px-4 py-2 transition duration-100 ease-in-out focus:border-blue-500 focus:ring-2 focus:ring-blue-500 focus:outline-none focus:ring-opacity-50 disabled:opacity-50 disabled:cursor-not-allowed',
            classes: 'text-white bg-blue-500 border border-transparent shadow-sm rounded hover:bg-blue-600',
            variants: {
                secondary: 'text-gray-800 bg-white border border-gray-300 shadow-sm hover:text-gray-600',
                error: 'text-white bg-red-500 border border-transparent rounded shadow-sm hover:bg-red-600',
                success: 'text-white bg-green-500 border border-transparent rounded shadow-sm hover:bg-green-600',
                link: 'text-blue-500 underline hover:text-blue-600',
                dark: 'p-2 lg:px-4 md:mx-2 text-indigo-600 text-center border border-transparent rounded hover:bg-indigo-100 hover:text-indigo-700 transition-colors duration-300',
            }
        }
    },
}

Vue.use(VueTailwind, settings)
Vue.component('answer-field', AnswerField)
Vue.component('vote', Vote)
Vue.component('statistics', Statistics)
Vue.component('loader', Loader)

Vue.config.devtools = process.env.NODE_ENV === 'development';
Vue.config.productionTip = process.env.NODE_ENV === 'development';

var app = new Vue({
    el: '#app',
    data: {
    }
});
