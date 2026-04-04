// import { useAuthStore } from '../state/AuthState'

export const fetch = (
	input: RequestInfo | URL,
	init: RequestInit = {},
): Promise<Response> => {
	// const token = useAuthStore.getState().account.token
	const token = "" // --- IGNORE ---
	const headers = new Headers(init.headers)
	if (token) {
		headers.set('Authorization', `Bearer ${token}`)
	}
	return fetch(input, { ...init, headers })
}
