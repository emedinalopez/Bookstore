import * as Keycloak from 'keycloak-js';

const keycloak = new Keycloak.default({
    url: 'http://localhost:8080',
    realm: 'BookstoreRealm',
    clientId: 'bookstore-api'
});

export default keycloak;
