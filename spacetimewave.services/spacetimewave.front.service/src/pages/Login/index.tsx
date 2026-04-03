import { useState, useEffect } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import styles from './index.module.css'
import logo from '../../assets/images/logo.png'
import Button from '../../components/Button'
import Input from '../../components/Input'
import CrossIcon from '../../assets/icons/CrossIcon'
import { login, navigateToRegistration, exchangeAuthCode } from '../../services/AuthService'

export default function Login() {
	const navigate = useNavigate()
	const [username, setUsername] = useState('')
	const [password, setPassword] = useState('')
	const [error, setError] = useState<string | null>(null)
	const [loading, setLoading] = useState(false)

	useEffect(() => {
		const params = new URLSearchParams(window.location.search)
		const code = params.get('code')
		if (!code) return
		window.history.replaceState({}, '', '/login')
		setLoading(true)
		exchangeAuthCode(code).then((ok) => {
			setLoading(false)
			if (ok) navigate('/account/feed')
			else setError('Registration succeeded but sign-in failed. Please log in manually.')
		})
	}, [])

	const handleLogin = async () => {
		if (!username || !password) {
			setError('Please enter your username and password.')
			return
		}
		setError(null)
		setLoading(true)
		const ok = await login(username, password)
		setLoading(false)
		if (ok) {
			navigate('/account/feed')
		} else {
			setError('Invalid credentials. Please try again.')
		}
	}

	return (
		<div className={styles.container}>
			<header className={styles.header}>
				<Link to={'/'} className={styles.cross}>
					<CrossIcon width='35px' height='35px' />
				</Link>
				<figure className={styles.figure}>
					<img src={logo} className={styles.logo} alt='Raptor logo' />
				</figure>
			</header>
			<main className={styles.main}>
				<h2 className={styles.title}>Sign in to Raptor</h2>
				<form
					onSubmit={(e) => {
						e.preventDefault()
						handleLogin()
					}}
				>
					<label>Username</label>
					<Input
						type='text'
						onChange={(e:any) => {e.preventDefault(); setUsername(e.target.value)}}
						autoComplete='username'
					/>
					<label>Password</label>
					<Input
						type='password'
						onChange={(e:any) => {e.preventDefault(); setPassword(e.target.value)}}
						autoComplete='current-password'
					/>
					{error && <p className={styles.error}>{error}</p>}
					<Button color='black' onClick={handleLogin}>
						{loading ? 'Signing in…' : 'Sign in'}
					</Button>
				</form>
				<p>
					Don't have an account?{' '}
					<a onClick={navigateToRegistration} style={{ cursor: 'pointer' }}>
						Sign up
					</a>
				</p>
			</main>
		</div>
	)
}
