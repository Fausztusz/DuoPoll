const defaultTheme = require('tailwindcss/defaultTheme')

module.exports = {
    purge: [
        './Resources/**/*.css',
        './Resources/**/*.js',
        './Views/**/*.cshtml'
    ],
    darkMode: 'class', // false or 'media' or 'class'
    theme: {
        fontFamily: {
            'display': ['Copse', 'Oswald', ...defaultTheme.fontFamily.sans],
            'body': ['Roboto Slab', 'Roboto', 'Open Sans', ...defaultTheme.fontFamily.serif],
            'sans': ['Open Sans', ...defaultTheme.fontFamily.sans],
        },
        borderRadius: {
            'none': '0',
            'sm': '0.125rem',
            'md': '0.375rem',
            'lg': '0.5rem',
            'full': '9999px',
            'large': '20px',
        },
        extend: {},
    },
    variants: {
        extend: {
            dropShadow: ['hover', 'focus'],
            // Vue forms
            opacity: ['disabled'],
            cursor: ['disabled'],
        },
    },
    plugins: [
        require('@tailwindcss/forms'),
    ],
}
