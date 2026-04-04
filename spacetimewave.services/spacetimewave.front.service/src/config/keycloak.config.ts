import Keycloak from 'keycloak-js'

const keycloak = new Keycloak({
	url: import.meta.env.VITE_IDENTITY_PROVIDER_URL,
	realm: import.meta.env.VITE_IDENTITY_PROVIDER_TENANT_ID,
	clientId: import.meta.env.VITE_IDENTITY_PROVIDER_CLIENT_ID,
})

export default keycloak
