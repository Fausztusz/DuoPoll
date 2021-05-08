import Vue from "vue";
import VueI18n from 'vue-i18n'
import translationEN from "../../lang/en.json";
import translationHU from "../../lang/hu.json";

Vue.use(VueI18n);

function getLang(lang){
    return getCookie(".AspNetCore.Culture").includes(lang)
}

function getCookie(cname) {
    let name = cname + "=";
    let ca = document.cookie.split(';');
    for(let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) === ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) === 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

export const i18n = new VueI18n(
    {
        locale: getLang("hu")?"hu":"en",
        fallbackLocale: 'en',
        messages: {
            en: translationEN,
            hu: translationHU,
        }
    }
)
