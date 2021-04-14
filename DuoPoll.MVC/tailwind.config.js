module.exports = {
    purge: [
        './Resources/**/*.css',
        './Resources/**/*.js',
        './Views/**/*.cshtml'
    ],
    darkMode: 'media', // false or 'media' or 'class'
    theme: {
        extend: {},
    },
    variants: {
        extend: {
            dropShadow: ['hover', 'focus'],
        },
    },
    plugins: [],
}
