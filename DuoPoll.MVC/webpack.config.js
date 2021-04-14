const {VueLoaderPlugin} = require('vue-loader');
const path = require('path');

module.exports = {
    mode: process.env.NODE_ENV,
    entry: path.resolve(__dirname, './Resources/js/app.js'),
    output: {
        path: path.resolve(__dirname, "./wwwroot/js/"),
        filename: "app.js",
    },
    resolve: {
        alias: {
            'vue$': 'vue/dist/vue.esm.js'
        },
        extensions: ['*', '.js', '.vue', '.json']
    },
    module: {
        rules: [
            {
                test: /\.vue$/,
                loader: 'vue-loader'
            },
            // this will apply to both plain `.js` files
            // AND `<script>` blocks in `.vue` files
            {
                test: /\.js$/,
                loader: 'babel-loader'
            },
            // this will apply to both plain `.css` files
            // AND `<style>` blocks in `.vue` files
            {
                test: /\.css$/,
                use: [
                    'vue-style-loader',
                    'css-loader'
                ],
            }
        ]
    },
}