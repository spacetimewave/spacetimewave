import styles from './index.module.css'
import { useAuthStore } from '../../state/AuthStore'
import MobileNav from '../../components/MobileNav'


export default function Feed() {
	const { account: { username } } = useAuthStore()
	return (
		<div className={styles.container}>
			<header className={styles.header}></header>
			<main className={styles.main}>
				<h1>Feed</h1>
				<div>Username: {username}</div>
			</main>
			<MobileNav />
		</div>
	)
}
