/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        'primary': '#04162e',
        'primary-container': '#1a2b44',
        'on-primary': '#ffffff',
        'on-primary-container': '#8292b0',
        'secondary': '#755a34',
        'secondary-container': '#fdd7a7',
        'surface': '#fbf9f8',
        'surface-container': '#efeded',
        'surface-container-low': '#f5f3f3',
        'surface-container-high': '#eae8e7',
        'on-surface': '#1b1c1c',
        'on-surface-variant': '#44474d',
        'outline': '#75777e',
        'outline-variant': '#c5c6ce',
        'error': '#ba1a1a',
        'error-container': '#ffdad6',
      },
      fontFamily: {
        'display': ['Playfair Display', 'serif'],
        'body': ['Montserrat', 'sans-serif'],
      },
      borderRadius: {
        'lg': '16px',
        'xl': '24px',
        'full': '9999px',
      }
    }
  },
  plugins: [],
}
