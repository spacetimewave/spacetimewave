import { create } from 'zustand'
import { persist } from 'zustand/middleware'

export const useCredentialStore = create(
	persist<IAuthStore>(
		(set) => ({
			username: null,
			usermail: null,
			token: null,
			refreshToken: null,
			idToken: null,
			setUsername: (key: string | null) => set({ username: key }),
			setUsermail: (key: string | null) => set({ usermail: key }),
			setToken: (key: string | null) => set({ token: key }),
			setRefreshToken: (key: string | null) => set({ refreshToken: key }),
			setIdToken: (key: string | null) => set({ idToken: key }),
		}),
		{
			name: 'auth',
		},
	),
)

export interface IAuthStore {
	username: string | null
	usermail: string | null
	token: string | null
	refreshToken: string | null
	idToken: string | null
	setUsername: (username: string | null) => void
	setUsermail: (usermail: string | null) => void
	setToken: (token: string | null) => void
	setRefreshToken: (refreshToken: string | null) => void
	setIdToken: (idToken: string | null) => void
}
