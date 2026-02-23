import MobileNav from '../../components/MobileNav'
import styles from './index.module.css'

export default function Messages() {
	return (
		<div className={styles.container}>
			<header className={styles.header}></header>
			<main className={styles.main}>
				<h1 className={styles.title}>Messages</h1>
			</main>
			<MobileNav />
		</div>
	)
}
