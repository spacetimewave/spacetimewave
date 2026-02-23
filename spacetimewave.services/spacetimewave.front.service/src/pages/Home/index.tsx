import { Link } from 'react-router-dom'
import styles from './index.module.css'
import logo from '../../assets/images/logo.png'
import WalletIcon from '../../assets/icons/WalletIcon'
import Button from '../../components/Button'

export default function Home() {
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
				<Link to={'/login'} className={styles.link}>
					<Button color='white'>
						<WalletIcon height='16px' width='16px' color='#1e8eda' /> Sign in
						with Raptor
					</Button>
				</Link>
				<div className={styles.line_wrapper}>
					<hr className={styles.line} />
					or
					<hr className={styles.line} />
				</div>
				<Link to={'/signup'}>
					<Button color='blue'>Create account</Button>
				</Link>
				<p className={styles.terms}>
					By signing up, you agree to the <a href='#'>Terms of Service</a> and{' '}
					<a href='#'>Privacy Policy</a>, including <a href='#'>Cookie Use</a>.
				</p>
			</main>
		</div>
	)
}
