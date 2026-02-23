// https://ethereum.stackexchange.com/questions/13652/how-do-you-sign-an-verify-a-message-in-javascript-proving-you-own-an-ethereum-ad

import { Link, useNavigate } from 'react-router-dom'
import styles from './index.module.css'
import logo from '../../assets/images/logo.png'
import Button from '../../components/Button'
import Input from '../../components/Input'
import CrossIcon from '../../assets/icons/CrossIcon'
import { useCredentialStore } from '../../state/AuthState'

export default function Login() {
	const navigate = useNavigate()
	const { setUsername, setPassword } = useCredentialStore()

	const login = (username, password) => {
		setUsername(username)
		setPassword(password)
		navigate('/feed')
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
				<form>
					<label>Password</label>
					<Input
						onChange={(ev) => {
							ev.preventDefault()
						}}
					></Input>
					<Button
						color='black'
						onClick={() => {
							login('javierhersan', '12345')
						}}
					>
						Next
					</Button>
				</form>
				<p>
					Don't have an account? <a href='/signup'>Sign up</a>
				</p>
			</main>
		</div>
	)
}
