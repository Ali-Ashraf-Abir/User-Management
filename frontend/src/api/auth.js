import api from './axios'

export const registerUser = (data) => api.post('/Auth/register', data)

export const loginUser = (data) => api.post('/Auth/login', data)

export const verifyEmail = (token) => api.get(`/Users/verify/${token}`)
