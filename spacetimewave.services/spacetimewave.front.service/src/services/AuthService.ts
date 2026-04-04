import Keycloak from 'keycloak-js'

export class AuthService {

	private _keycloak: Keycloak

	constructor(keycloak: Keycloak) {
		this._keycloak = keycloak
	}

	async init(): Promise<void> {
		if (this._keycloak.didInitialize) return
		await this._keycloak.init({ pkceMethod: 'S256', onLoad: 'check-sso' })
	}

	login() {
		return this._keycloak.login({ redirectUri: `${window.location.origin}/account/feed` })
	}

	register() {
		return this._keycloak.register({ redirectUri: `${window.location.origin}/account/feed` })
	}

	logout() {
		return this._keycloak.logout({ redirectUri: window.location.origin })
	}

	isAuthenticated(): boolean {
		return !!this._keycloak.authenticated
	}

	getTokenParsed() {
		return this._keycloak.tokenParsed
	}

	onTokenExpired(cb: () => void) {
		this._keycloak.onTokenExpired = cb
	}

	updateToken(minValidity: number) {
		return this._keycloak.updateToken(minValidity)
	}
}
