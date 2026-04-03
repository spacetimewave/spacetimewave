import { useEffect } from 'react'

const KEYCLOAK_URL = import.meta.env.VITE_IDENTITY_PROVIDER_URL
const KEYCLOAK_REALM = import.meta.env.VITE_IDENTITY_PROVIDER_TENANT_ID
const KEYCLOAK_CLIENT_ID = import.meta.env.VITE_IDENTITY_PROVIDER_CLIENT_ID

export default function Signup() {
	useEffect(() => {
		const redirectUri = `${window.location.origin}/login`
		window.location.href =
			`${KEYCLOAK_URL}/realms/${KEYCLOAK_REALM}/protocol/openid-connect/auth` +
			`?client_id=${KEYCLOAK_CLIENT_ID}` +
			`&response_type=code` +
			`&scope=openid` +
			`&prompt=create` +
			`&redirect_uri=${encodeURIComponent(redirectUri)}`
	}, [])

	return null
}
