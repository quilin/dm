const path = require('path');

module.exports = {
  css: {
    loaderOptions: {
      stylus: {
        import: [
          path.resolve('./src/styles/Reset.styl'),
          path.resolve('./src/styles/Variables.styl'),
          path.resolve('./src/styles/Themes.styl'),
          path.resolve('./src/styles/Fonts.styl'),
          path.resolve('./src/styles/Blocks.styl'),
          path.resolve('./src/styles/Inputs.styl'),
        ]
      }
    }
  }
};
