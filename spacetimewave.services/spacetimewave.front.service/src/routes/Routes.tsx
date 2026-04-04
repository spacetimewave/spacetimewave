import ProtectedRoute from './ProtectedRoute'
import Home from '../pages/Home'
import Error from '../pages/Error'
import Layout from '../components/Layout'
import Feed from '../pages/Feed'
import Search from '../pages/Search'
import Profile from '../pages/Profile'
import Messages from '../pages/Messages'

export const Routes = [
	{
		path: '/',
		element: (
			<Home />
		),
		errorElement: <Error />,
	},
	{
		path: '/account',
		element: (
			<ProtectedRoute>
				<Layout />
			</ProtectedRoute>
		),
		errorElement: <Error />,
		children: [
			{
				path: '/account/feed',
				element: <Feed />,
			},
			{
				path: '/account/search',
				element: <Search />,
			},
			{
				path: '/account/messages',
				element: <Messages />,
			},
			{
				path: '/account/profile',
				element: <Profile />,
			},
		],
	},
]
