import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import { ReactKeycloakProvider } from '@react-keycloak/web';
import keycloak from './keycloak';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

root.render(
  
  <ReactKeycloakProvider 
    authClient={keycloak}
    initOptions={{ onLoad: 'login-required' }}
  >
    <App />
  </ReactKeycloakProvider>
);
