import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { verifyEmail } from '../api/auth'

export default function VerifyEmail() {
  const { token } = useParams()
  const [status, setStatus] = useState('loading') // loading | success | error
  const [message, setMessage] = useState('')
  const [user, setUser] = useState(null)

  useEffect(() => {
    let active = true
    const run = async () => {
      try {
        const { data } = await verifyEmail(token)
        if (!active) return
        setUser(data)
        setStatus('success')
      } catch (err) {
        if (!active) return
        setMessage(err.response?.data?.message || 'Verification failed')
        setStatus('error')
      }
    }
    run()
    return () => {
      active = false
    }
  }, [token])

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 px-4">
      <div className="w-full max-w-md bg-white shadow-md rounded-lg p-8 text-center">
        {status === 'loading' && (
          <>
            <div className="mx-auto mb-4 h-10 w-10 animate-spin rounded-full border-4 border-blue-200 border-t-blue-600" />
            <h1 className="text-xl font-semibold text-gray-800">
              Verifying your email...
            </h1>
          </>
        )}

        {status === 'success' && (
          <>
            <div className="mx-auto mb-4 flex h-14 w-14 items-center justify-center rounded-full bg-green-100">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                className="h-7 w-7 text-green-600"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
                strokeWidth={2}
              >
                <path strokeLinecap="round" strokeLinejoin="round" d="M5 13l4 4L19 7" />
              </svg>
            </div>
            <h1 className="text-2xl font-bold text-gray-800 mb-2">
              Email Verified!
            </h1>
            <p className="text-gray-600 mb-6">
              {user?.fullName ? `Hi ${user.fullName}, ` : ''}
              your email has been successfully verified. You can now sign in to your account.
            </p>
            <Link
              to="/login"
              className="inline-block w-full bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 rounded-md transition-colors"
            >
              Go to Sign in
            </Link>
          </>
        )}

        {status === 'error' && (
          <>
            <div className="mx-auto mb-4 flex h-14 w-14 items-center justify-center rounded-full bg-red-100">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                className="h-7 w-7 text-red-600"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
                strokeWidth={2}
              >
                <path strokeLinecap="round" strokeLinejoin="round" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </div>
            <h1 className="text-2xl font-bold text-gray-800 mb-2">
              Verification Failed
            </h1>
            <p className="text-gray-600 mb-6">{message}</p>
            <Link
              to="/login"
              className="inline-block w-full bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 rounded-md transition-colors"
            >
              Back to Sign in
            </Link>
          </>
        )}
      </div>
    </div>
  )
}
