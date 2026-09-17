import { defineConfig } from 'orval';

export default defineConfig({
  bookapp: {
    input: 'http://localhost:5169/openapi/v1.json',
    output: {
      mode: 'split',
      target: './src/app/core/api/bookapp.ts',
      schemas: './src/app/core/api/model',
      client: 'angular',
      clean: true,
    },
  },
});
