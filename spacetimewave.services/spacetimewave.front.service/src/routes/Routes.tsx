import Error from '../pages/Error'
import Home from '../pages/Home'
import Login from '../pages/Login'
import Layout from '../components/Layout'
import Feed from '../pages/Feed'
import Search from '../pages/Search'
import Profile from '../pages/Profile'
import Messages from '../pages/Messages'
import ProtectedRoute from './ProtectedRoute'

export const Routes = [
	{
		path: '/',
		element: <Home />,
		errorElement: <Error />,
	},
	{
		path: '/login',
		element: <Login />,
		errorElement: <Error />,
	},
	{
		path: '/',
		element: (
			<ProtectedRoute>
				<Layout />
			</ProtectedRoute>
		),
		errorElement: <Error />,
		children: [
			{
				path: '/feed',
				element: <Feed />,
			},
			{
				path: '/search',
				element: <Search />,
			},
			{
				path: '/messages',
				element: <Messages />,
			},
			{
				path: '/profile',
				element: <Profile />,
			},
		],
	},
]
