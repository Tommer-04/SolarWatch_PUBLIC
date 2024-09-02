import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': {
        target: '[BACKEND_URL]',
        changeOrigin: true,
        secure: false
      }
    }
  }
});