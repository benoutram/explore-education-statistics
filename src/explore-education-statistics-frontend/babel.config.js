module.exports = {
  presets: [
    [
      'next/babel',
      {
        'preset-env': {
          useBuiltIns: 'usage',
          targets: {
            browsers: ['ie>=11', 'safari>=12', 'ios>=9.3', 'last 1 version'],
          },
        },
      },
    ],
    '@zeit/next-typescript/babel',
  ],
};
