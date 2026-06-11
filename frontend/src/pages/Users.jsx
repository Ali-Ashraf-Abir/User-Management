import { useEffect, useMemo, useState } from 'react'
import {
  getUsers,
  blockUsers,
  unblockUsers,
  deleteUsers,
  deleteUnverifiedUsers,
} from '../api/users'
import { useAuth } from '../context/AuthContext'
import UsersToolbar from '../components/UsersToolbar'
import { timeAgo, formatDateTime } from '../utils/time'

const ACCOUNT_STATUS = {
  0: 'Unverified',
  1: 'Verified',
  // fallback for string enums returned as text
  Unverified: 'Unverified',
  Verified: 'Verified',
}

function statusLabel(user) {
  if (user.isBlocked) return 'Blocked'
  const status = ACCOUNT_STATUS[user.accountStatus] ?? user.accountStatus
  if (status === 'Verified') return 'Active'
  return status || 'Unverified'
}

function statusBadgeClass(label) {
  switch (label) {
    case 'Active':
      return 'text-green-700'
    case 'Blocked':
      return 'text-gray-400'
    case 'Unverified':
      return 'text-amber-600'
    default:
      return 'text-gray-600'
  }
}

function isUnverified(user) {
  const status = ACCOUNT_STATUS[user.accountStatus] ?? user.accountStatus
  return status === 'Unverified'
}

export default function Users() {
  const { user: currentUser, logout } = useAuth()
  const [users, setUsers] = useState([])
  const [selected, setSelected] = useState(new Set())
  const [filter, setFilter] = useState('')
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [actionLoading, setActionLoading] = useState(false)

  const fetchUsers = async () => {
    setLoading(true)
    setError('')
    try {
      const { data } = await getUsers()
      setUsers(data)
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to load users')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    fetchUsers()
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  const filteredUsers = useMemo(() => {
    const term = filter.trim().toLowerCase()
    if (!term) return users
    return users.filter(
      (u) =>
        u.fullName?.toLowerCase().includes(term) ||
        u.email?.toLowerCase().includes(term)
    )
  }, [users, filter])

  const allSelected =
    filteredUsers.length > 0 && filteredUsers.every((u) => selected.has(u.id))
  const someSelected = filteredUsers.some((u) => selected.has(u.id))

  const toggleAll = () => {
    setSelected((prev) => {
      const next = new Set(prev)
      if (allSelected) {
        filteredUsers.forEach((u) => next.delete(u.id))
      } else {
        filteredUsers.forEach((u) => next.add(u.id))
      }
      return next
    })
  }

  const toggleOne = (id) => {
    setSelected((prev) => {
      const next = new Set(prev)
      if (next.has(id)) next.delete(id)
      else next.add(id)
      return next
    })
  }

  const selectedUsers = users.filter((u) => selected.has(u.id))
  const hasBlockable = selectedUsers.some((u) => !u.isBlocked)
  const hasUnblockable = selectedUsers.some((u) => u.isBlocked)

  const runAction = async (fn, successMsg) => {
    setActionLoading(true)
    setError('')
    try {
      await fn()
      setSelected(new Set())
      await fetchUsers()
    } catch (err) {
      setError(err.response?.data?.message || 'Action failed')
    } finally {
      setActionLoading(false)
    }
  }

  const handleBlock = () => {
    const ids = selectedUsers.filter((u) => !u.isBlocked).map((u) => u.id)
    if (ids.length === 0) return
    runAction(() => blockUsers(ids))
  }

  const handleUnblock = () => {
    const ids = selectedUsers.filter((u) => u.isBlocked).map((u) => u.id)
    if (ids.length === 0) return
    runAction(() => unblockUsers(ids))
  }

  const handleDelete = () => {
    const ids = [...selected]
    if (ids.length === 0) return
    if (!window.confirm(`Delete ${ids.length} selected user(s)? This cannot be undone.`)) return
    runAction(() => deleteUsers(ids))
  }

  const handleDeleteUnverified = () => {
    if (!window.confirm('Delete all unverified users? This cannot be undone.')) return
    runAction(() => deleteUnverifiedUsers())
  }

  return (
    <div className="min-h-screen bg-gray-100">
      <header className="bg-white border-b border-gray-200">
        <div className="max-w-6xl mx-auto px-4 py-3 flex items-center justify-between">
          <h1 className="text-lg font-semibold text-gray-800">User Management</h1>
          <div className="flex items-center gap-3">
            {currentUser?.fullName && (
              <span className="text-sm text-gray-600">{currentUser.fullName}</span>
            )}
            <button
              onClick={logout}
              className="text-sm font-medium text-gray-600 hover:text-gray-900"
            >
              Sign out
            </button>
          </div>
        </div>
      </header>

      <main className="max-w-6xl mx-auto px-4 py-6">
        {error && (
          <div className="mb-4 rounded-md bg-red-50 border border-red-200 text-red-700 px-4 py-2 text-sm">
            {error}
          </div>
        )}

        <div className="bg-white rounded-lg shadow-sm overflow-hidden">
          <UsersToolbar
            selectedCount={selected.size}
            hasBlockable={hasBlockable}
            hasUnblockable={hasUnblockable}
            onBlock={handleBlock}
            onUnblock={handleUnblock}
            onDelete={handleDelete}
            onDeleteUnverified={handleDeleteUnverified}
            filter={filter}
            onFilterChange={setFilter}
          />

          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-white">
                <tr>
                  <th className="w-10 px-4 py-3 text-left">
                    <input
                      type="checkbox"
                      checked={allSelected}
                      ref={(el) => {
                        if (el) el.indeterminate = !allSelected && someSelected
                      }}
                      onChange={toggleAll}
                      disabled={actionLoading || loading}
                      className="h-4 w-4 rounded border-gray-300 text-blue-600 focus:ring-blue-500"
                    />
                  </th>
                  <th className="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wider">
                    Name
                  </th>
                  <th className="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wider">
                    Email
                  </th>
                  <th className="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wider">
                    Status
                  </th>
                  <th className="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wider">
                    Last seen
                  </th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100">
                {loading && (
                  <tr>
                    <td colSpan={5} className="px-4 py-8 text-center text-sm text-gray-500">
                      Loading users...
                    </td>
                  </tr>
                )}

                {!loading && filteredUsers.length === 0 && (
                  <tr>
                    <td colSpan={5} className="px-4 py-8 text-center text-sm text-gray-500">
                      No users found.
                    </td>
                  </tr>
                )}

                {!loading &&
                  filteredUsers.map((u) => {
                    const label = statusLabel(u)
                    const isBlocked = u.isBlocked
                    return (
                      <tr key={u.id} className={isBlocked ? 'bg-gray-50' : 'bg-white'}>
                        <td className="px-4 py-3 align-top">
                          <input
                            type="checkbox"
                            checked={selected.has(u.id)}
                            onChange={() => toggleOne(u.id)}
                            disabled={actionLoading}
                            className="h-4 w-4 rounded border-gray-300 text-blue-600 focus:ring-blue-500"
                          />
                        </td>
                        <td className="px-4 py-3 align-top">
                          <div
                            className={`text-sm font-medium ${
                              isBlocked ? 'text-gray-400 line-through' : 'text-gray-900'
                            }`}
                          >
                            {u.fullName}
                          </div>
                        </td>
                        <td className="px-4 py-3 align-top">
                          <span
                            className={`text-sm ${
                              isBlocked ? 'text-gray-400' : 'text-gray-700'
                            }`}
                          >
                            {u.email}
                          </span>
                        </td>
                        <td className="px-4 py-3 align-top">
                          <span className={`text-sm font-medium ${statusBadgeClass(label)}`}>
                            {label}
                          </span>
                        </td>
                        <td className="px-4 py-3 align-top">
                          <span
                            className={`text-sm ${isBlocked ? 'text-gray-400' : 'text-gray-600'}`}
                            title={formatDateTime(u.lastLoginTime)}
                          >
                            {timeAgo(u.lastLoginTime)}
                          </span>
                        </td>
                      </tr>
                    )
                  })}
              </tbody>
            </table>
          </div>
        </div>
      </main>
    </div>
  )
}
