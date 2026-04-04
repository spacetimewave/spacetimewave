import { create } from 'zustand'
import keycloak from '../config/keycloak.config'
import { AuthService } from '../services/AuthService'

const authService = new AuthService(keycloak)

interface Account {
	username: string | null
	usermail: string | null
	isAuthenticated: boolean
}

export interface IAuthStore {
	account: Account
	loading: boolean
	init: () => Promise<void>
	login: () => Promise<void>
	register: () => Promise<void>
	logout: () => Promise<void>
}

export const useAuthStore = create<IAuthStore>((set) => ({
	account: {
		username: null,
		usermail: null,
		isAuthenticated: false,
	},
	loading: true,

	init: async () => {
		await authService.init()
		set({ account: buildAccount(), loading: false })
		authService.onTokenExpired(() => {
			authService.updateToken(30)
				.then(() => set({ account: buildAccount() }))
				.catch(() => set({ account: buildAccount() }))
		})
	},

	login: () => authService.login(),

	register: () => authService.register(),

	logout: () => authService.logout(),
}))

function buildAccount(): Account {
	if (authService.isAuthenticated()) {
		const token = authService.getTokenParsed()!
		return {
			username: token.preferred_username,
			usermail: token.email,
			isAuthenticated: true,
		}
	}
	return { username: null, usermail: null, isAuthenticated: false }
}
