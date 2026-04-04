import { useNavigate } from 'react-router-dom'
import styles from './index.module.css'
import logo from '../../assets/images/logo.png'
import Button from '../../components/Button'
import { useAuthStore } from '../../state/AuthStore'

export default function Home() {
	const navigate = useNavigate()
	const { account, login, register } = useAuthStore()

	const handleRegister = () => {
		if (account.isAuthenticated) {
			navigate('/account/feed')
		} else {
			register()
		}
	}

	return (
		<div className={styles.container}>
			<header className={styles.header}>
				<figure className={styles.figure}>
					<img src={logo} className={styles.logo} alt='Raptor logo' />
				</figure>
				<h1>Happening now</h1>
				<h2>Join today.</h2>
			</header>
			<main className={styles.main}>
				<Button color='white' onClick={login}>
					Sign in with Raptor
				</Button>
				<div className={styles.line_wrapper}>
					<hr className={styles.line} />
					or
					<hr className={styles.line} />
				</div>
				<Button color='blue' onClick={handleRegister}>
					Create account
				</Button>
				<p className={styles.terms}>
					By signing up, you agree to the <a href='#'>Terms of Service</a> and{' '}
					<a href='#'>Privacy Policy</a>, including <a href='#'>Cookie Use</a>.
				</p>
			</main>
		</div>
	)
}
