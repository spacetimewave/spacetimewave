import { Navigate } from 'react-router-dom'

import { useCredentialStore } from '../state/AuthState'
import type { ReactNode } from 'react'

interface Props {
	children: ReactNode
}

function ProtectedRoute({ children }: Props) {
	const { token } = useCredentialStore()
	if (!token) {
		return <Navigate to='/' replace />
	}
	return children
}

export default ProtectedRoute