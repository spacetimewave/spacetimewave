import { Navigate } from 'react-router-dom'

import { useAuthStore } from '../state/AuthStore'
import type { ReactNode } from 'react'

interface Props {
	children: ReactNode
}

function ProtectedRoute({ children }: Props) {
	const { account, loading } = useAuthStore()
	if (loading) return null
	if (!account.isAuthenticated) {
		return <Navigate to='/' replace />
	}
	return children
}

export default ProtectedRoute