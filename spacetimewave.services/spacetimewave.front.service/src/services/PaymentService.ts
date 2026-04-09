import { apiFetch } from './ApiService'

const API_URL = import.meta.env.VITE_API_URL


export const SubscriptionPlan = {
	Free: 'Free',
	Pro: 'Pro',
} as const;
export type SubscriptionPlan = typeof SubscriptionPlan[keyof typeof SubscriptionPlan];

export const SubscriptionStatus = {
	Active: 'Active',
	Inactive: 'Inactive',
	Canceled: 'Canceled',
} as const;
export type SubscriptionStatus = typeof SubscriptionStatus[keyof typeof SubscriptionStatus];

export interface Subscription {
	userId: string;
	plan: SubscriptionPlan;
	status: SubscriptionStatus;
	updatedAt: string | null;
}

export const PaymentService = {
	async createCheckoutSession(): Promise<string> {
		const successUrl = `${window.location.origin}/account/profile?success=true`
		const cancelUrl = `${window.location.origin}/account/profile?canceled=true`

		const response = await apiFetch(`${API_URL}/api/payments/checkout`, {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({ successUrl, cancelUrl }),
		})

		if (!response.ok) throw new Error('Failed to create checkout session')

		const data = await response.json()
		return data.url as string
	},

	async getSubscription(): Promise<Subscription> {
		const response = await apiFetch(`${API_URL}/api/payments/subscription`)
		if (!response.ok) throw new Error('Failed to fetch subscription')
		return response.json()
	},
}
