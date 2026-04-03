import styles from './index.module.css'

export default function Input({ onChange = (e:any) => {e.preventDefault(); onChange(e)}, type = 'text', autoComplete = 'off' }) {
	return (
		<input
			type={type}
			placeholder=''
			onChange={onChange}
			autoComplete={autoComplete}
			className={styles.input}
		></input>
	)
}
