import Vue from "vue";
import VueTailwind from 'vue-tailwind'
import TInput from "vue-tailwind/dist/t-input";
import TCheckbox from "vue-tailwind/dist/t-checkbox";
import TDatepicker from "vue-tailwind/dist/t-datepicker";
import TButton from "vue-tailwind/dist/t-button";

import Hungarian from "vue-tailwind/dist/l10n/hu";
import English from "vue-tailwind/dist/l10n/default";

window.Vue = require("vue");

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
    't-button': {component: TButton},
}

Vue.use(VueTailwind, settings)

Vue.config.devtools = process.env.NODE_ENV === 'development';
Vue.config.productionTip = process.env.NODE_ENV === 'development';

var app = new Vue({
    el: '#app',
    data: {
        lastname: '',
        firstname: '',
    }
});
