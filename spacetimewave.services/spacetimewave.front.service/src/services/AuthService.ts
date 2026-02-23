import { useCredentialStore } from '../state/AuthState'

const { setUsername, setUsermail, setPassword, setToken } =
	useCredentialStore.getState()

export const signup = async (
	username: string,
	email: string,
	password: string,
): Promise<boolean> => {
	try {
		const response = await fetch(`${import.meta.env.VITE_API_URL}/signup`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
			},
			body: JSON.stringify({
				username,
				email,
				password,
			}),
		})

		if (!response.ok) {
			const errorData = await response.json()
			throw new Error(errorData.message || 'Failed to sign up')
		}

		// If signup is successful
		const data = await response.json()
		setUsername(username)
		setUsermail(email)
		setPassword(password)
		setToken(data.access_token)
		return true
	} catch (error) {
		console.log(error)
		return false
	}
}

export const login = async (
	usermail: string,
	password: string,
): Promise<boolean> => {
	try {
		const response = await fetch(`${import.meta.env.VITE_API_URL}/login`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/x-www-form-urlencoded',
			},
			body: new URLSearchParams({
				username: usermail,
				password,
			}).toString(),
		})

		if (!response.ok) {
			const errorData = await response.json()
			throw new Error(errorData.message || 'Failed to log in')
		}

		// If login is successful
		const data = await response.json()
		setUsermail(usermail)
		setUsername(data.user_name)
		setPassword(password)
		setToken(data.access_token)
		return true
	} catch (error) {
		console.log(error)
		return false
	}
}

export const signout = async () => {
	setUsermail(null)
	setUsername(null)
	setPassword(null)
	setToken(null)
}
