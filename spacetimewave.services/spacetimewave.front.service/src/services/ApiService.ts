import { useAuthStore } from '../state/AuthStore'

export const apiFetch = (
	input: RequestInfo | URL,
	init: RequestInit = {},
): Promise<Response> => {
	const token = useAuthStore.getState().getToken()
	const headers = new Headers(init.headers)
	if (token) {
		headers.set('Authorization', `Bearer ${token}`)
	}
	return globalThis.fetch(input, { ...init, headers })
}
