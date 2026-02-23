import styles from './index.module.css'
import { useCredentialStore } from '../../state/AuthState'
import MobileNav from '../../components/MobileNav'


export default function Feed() {
	const { username, password } = useCredentialStore()
	return (
		<div className={styles.container}>
			<header className={styles.header}></header>
			<main className={styles.main}>
				<h1>Feed</h1>
				<div>Username: {username}</div>
				<div>Password: {password && password}</div>
			</main>
			<MobileNav />
		</div>
	)
}
