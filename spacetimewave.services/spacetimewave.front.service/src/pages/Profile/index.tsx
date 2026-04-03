import { useNavigate } from 'react-router-dom'
import { signout } from '../../services/AuthService'
import styles from './index.module.css'
import MobileNav from '../../components/MobileNav'
import Button from '../../components/Button'

export default function Profile() {
	const navigate = useNavigate()
	const handleSignout = async () => {
		await signout()
		navigate('/')
	}

	return (
		<div className={styles.container}>
			<header className={styles.header}></header>
			<main className={styles.main}>
				<h1 className={styles.title}>Profile</h1>
				<Button color='black' onClick={handleSignout}>
					Log out
				</Button>
			</main>
			<MobileNav />
		</div>
	)
}
