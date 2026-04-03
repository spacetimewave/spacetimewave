import { useCredentialStore } from '../state/AuthState'

const KEYCLOAK_URL = import.meta.env.VITE_IDENTITY_PROVIDER_URL
const KEYCLOAK_REALM = import.meta.env.VITE_IDENTITY_PROVIDER_TENANT_ID
const KEYCLOAK_CLIENT_ID = import.meta.env.VITE_IDENTITY_PROVIDER_CLIENT_ID
const TOKEN_URL = `${KEYCLOAK_URL}/realms/${KEYCLOAK_REALM}/protocol/openid-connect/token`
const LOGOUT_URL = `${KEYCLOAK_URL}/realms/${KEYCLOAK_REALM}/protocol/openid-connect/logout`

function decodeJwtPayload(token: string): Record<string, unknown> {
	try {
		return JSON.parse(atob(token.split('.')[1]))
	} catch {
		return {}
	}
}

export const login = async (
	username: string,
	password: string,
): Promise<boolean> => {
	try {
		const response = await fetch(TOKEN_URL, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/x-www-form-urlencoded',
			},
			body: new URLSearchParams({
				grant_type: 'password',
				client_id: KEYCLOAK_CLIENT_ID,
				scope: 'openid',
				username,
				password,
			}).toString(),
		})

		if (!response.ok) {
			const errorData = await response.json()
			throw new Error(errorData.error_description || 'Failed to log in')
		}

		const data = await response.json()
		const payload = decodeJwtPayload(data.access_token)

		const { setUsername, setUsermail, setToken, setRefreshToken, setIdToken } =
			useCredentialStore.getState()
		setToken(data.access_token)
		setRefreshToken(data.refresh_token ?? null)
		setIdToken(data.id_token ?? null)
		setUsername((payload.preferred_username as string) ?? username)
		setUsermail((payload.email as string) ?? null)
		return true
	} catch (error) {
		console.log(error)
		return false
	}
}

export const exchangeAuthCode = async (code: string): Promise<boolean> => {
	try {
		const redirectUri = `${window.location.origin}/login`
		const response = await fetch(TOKEN_URL, {
			method: 'POST',
			headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
			body: new URLSearchParams({
				grant_type: 'authorization_code',
				client_id: KEYCLOAK_CLIENT_ID,
				code,
				redirect_uri: redirectUri,
			}).toString(),
		})

		if (!response.ok) return false

		const data = await response.json()
		const payload = decodeJwtPayload(data.access_token)
		const { setUsername, setUsermail, setToken, setRefreshToken, setIdToken } =
			useCredentialStore.getState()
		setToken(data.access_token)
		setRefreshToken(data.refresh_token ?? null)
		setIdToken(data.id_token ?? null)
		setUsername((payload.preferred_username as string) ?? null)
		setUsermail((payload.email as string) ?? null)
		return true
	} catch {
		return false
	}
}

export const navigateToRegistration = () => {
	const redirectUri = `${window.location.origin}/login`
	window.location.href =
		`${KEYCLOAK_URL}/realms/${KEYCLOAK_REALM}/protocol/openid-connect/auth` +
		`?client_id=${KEYCLOAK_CLIENT_ID}` +
		`&response_type=code` +
		`&scope=openid` +
		`&prompt=create` +
		`&redirect_uri=${encodeURIComponent(redirectUri)}`
}

export const signout = async () => {
	const { refreshToken, setUsername, setUsermail, setToken, setRefreshToken, setIdToken } =
		useCredentialStore.getState()

	if (refreshToken) {
		try {
			await fetch(LOGOUT_URL, {
				method: 'POST',
				headers: {
					'Content-Type': 'application/x-www-form-urlencoded',
				},
				body: new URLSearchParams({
					client_id: KEYCLOAK_CLIENT_ID,
					refresh_token: refreshToken,
				}).toString(),
			})
		} catch (error) {
			console.log(error)
		}
	}

	setUsername(null)
	setUsermail(null)
	setToken(null)
	setRefreshToken(null)
	setIdToken(null)
}
