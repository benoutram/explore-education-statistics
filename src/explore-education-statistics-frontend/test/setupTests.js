import 'react-testing-library/cleanup-after-each';
import 'jest-dom/extend-expect';

if (typeof window !== 'undefined') {
  // fetch polyfill for making API calls.
  require('whatwg-fetch');
}

const localStorageMock = {
  getItem: jest.fn(),
  setItem: jest.fn(),
  clear: jest.fn(),
};
global.localStorage = localStorageMock;

Element.prototype.scrollIntoView = jest.fn();

afterEach(() => {
  location.hash = '';
});
