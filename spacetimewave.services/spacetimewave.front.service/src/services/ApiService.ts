import { useCredentialStore } from '../state/AuthState'

export const fetch = (
	input: RequestInfo | URL,
	init: RequestInit = {},
): Promise<Response> => {
	const token = useCredentialStore.getState().token
	const headers = new Headers(init.headers)
	if (token) {
		headers.set('Authorization', `Bearer ${token}`)
	}
	return fetch(input, { ...init, headers })
}
