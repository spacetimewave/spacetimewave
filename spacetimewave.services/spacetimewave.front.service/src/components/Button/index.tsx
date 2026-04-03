import styles from './index.module.css'

export default function Button({
	children = '',
	onClick = () => {},
	color = 'white',
}: {
	children?: React.ReactNode
	onClick?: () => void
	color?: 'white' | 'black' | 'blue'
}) {
	// color, size
	const whiteClass = color === 'white' ? styles.btn : ''
	const blackClass = color === 'black' ? styles.black_btn : ''
	const blueClass = color === 'blue' ? styles.blue_btn : ''
	const className = `${whiteClass} ${blackClass} ${blueClass}`

	return (
		<button className={className} onClick={onClick} type='button'>
			{children ?? children}
		</button>
	)
}
