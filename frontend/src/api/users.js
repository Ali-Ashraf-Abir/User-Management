import api from './axios'

export const getUsers = () => api.get('/Users')

export const blockUsers = (userIds) => api.post('/Users/block', { userIds })

export const unblockUsers = (userIds) => api.post('/Users/unblock', { userIds })

export const deleteUsers = (userIds) =>
  api.delete('/Users', { data: { userIds } })

export const deleteUnverifiedUsers = () => api.delete('/Users/unverified')
