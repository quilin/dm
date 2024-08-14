// eslint-disable-next-line @typescript-eslint/no-var-requires
const path = require('path');

module.exports = {
  css: {
    loaderOptions: {
      stylus: {
        import: [
          path.resolve('./src/styles/Variables.styl'),
          path.resolve('./src/styles/Themes.styl'),
          path.resolve('./src/styles/Fonts.styl'),
          path.resolve('./src/styles/Blocks.styl'),
          path.resolve('./src/styles/Inputs.styl'),
          path.resolve('./src/styles/Grid.styl')
        ]
      }
    }
  }
};
