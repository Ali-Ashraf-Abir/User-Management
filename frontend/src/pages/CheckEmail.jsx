import { Link, useLocation } from 'react-router-dom'

export default function CheckEmail() {
  const location = useLocation()
  const email = location.state?.email

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 px-4">
      <div className="w-full max-w-md bg-white shadow-md rounded-lg p-8 text-center">
        <div className="mx-auto mb-4 flex h-14 w-14 items-center justify-center rounded-full bg-blue-100">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            className="h-7 w-7 text-blue-600"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
            strokeWidth={2}
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z"
            />
          </svg>
        </div>

        <h1 className="text-2xl font-bold text-gray-800 mb-2">
          Verify your email
        </h1>

        <p className="text-gray-600 mb-1">
          We've sent a verification link to{' '}
          {email ? <span className="font-medium text-gray-800">{email}</span> : 'your email address'}.
        </p>
        <p className="text-gray-600 mb-6">
          Please check your inbox and click the link to activate your account.
        </p>

        <Link
          to="/login"
          className="inline-block w-full bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 rounded-md transition-colors"
        >
          Back to Sign in
        </Link>
      </div>
    </div>
  )
}
