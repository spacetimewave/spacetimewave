import { useEffect, useState } from 'react'
import { useNavigate, useSearchParams } from 'react-router-dom'
import { useAuthStore } from '../../state/AuthStore'
import { PaymentService, type Subscription } from '../../services/PaymentService'
import styles from './index.module.css'
import MobileNav from '../../components/MobileNav'
import Button from '../../components/Button'

export default function Profile() {
	const navigate = useNavigate()
	const [searchParams] = useSearchParams()
	const { logout, account } = useAuthStore()

	const [subscription, setSubscription] = useState<Subscription | null>(null)
	const [loadingSubscription, setLoadingSubscription] = useState(true)
	const [checkingOut, setCheckingOut] = useState(false)
	const [message, setMessage] = useState<string | null>(null)

	useEffect(() => {
		if (searchParams.get('success') === 'true')
			setMessage('Payment successful! Your PRO subscription is being activated.')
		else if (searchParams.get('canceled') === 'true')
			setMessage('Payment canceled.')
	}, [searchParams])

	useEffect(() => {
		PaymentService.getSubscription()
			.then(setSubscription)
			.catch(() => setSubscription(null))
			.finally(() => setLoadingSubscription(false))
	}, [])

	const handleSignout = async () => {
		await logout()
		navigate('/')
	}

	const handleSubscribe = async () => {
		setCheckingOut(true)
		try {
			const url = await PaymentService.createCheckoutSession()
			window.location.href = url
		} catch {
			setMessage('Failed to start checkout. Please try again.')
			setCheckingOut(false)
		}
	}

	const isPro = subscription?.plan === 'Pro' && subscription?.status === 'Active'

	return (
		<div className={styles.container}>
			<header className={styles.header}></header>
			<main className={styles.main}>
				<h1 className={styles.title}>Profile</h1>

				{account.username && (
					<p className={styles.username}>{account.username}</p>
				)}

				<div className={styles.subscription}>
					<div className={styles.planBadge} data-pro={isPro}>
						{isPro ? '★ PRO' : 'FREE'}
					</div>

					{!loadingSubscription && !isPro && (
						<Button color='blue' onClick={handleSubscribe}>
							{checkingOut ? 'Redirecting...' : 'Upgrade to PRO'}
						</Button>
					)}

					{isPro && (
						<p className={styles.proMessage}>You're on the PRO plan.</p>
					)}
				</div>

				{message && <p className={styles.message}>{message}</p>}

				<Button color='black' onClick={handleSignout}>
					Log out
				</Button>
			</main>
			<MobileNav />
		</div>
	)
}
