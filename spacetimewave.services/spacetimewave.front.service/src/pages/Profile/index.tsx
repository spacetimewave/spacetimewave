import MobileNav from '../../components/MobileNav'
import styles from './index.module.css'

export default function Profile() {
	return (
		<div className={styles.container}>
			<header className={styles.header}></header>
			<main className={styles.main}>
				<h1 className={styles.title}>Profile</h1>
			</main>
			<MobileNav />
		</div>
	)
}
