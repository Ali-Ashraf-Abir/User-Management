# Task4 Frontend

React + Vite + Tailwind CSS frontend for the Task4 user management API.

## Setup

```bash
npm install
npm run dev
```

## Configuration

Update the API base URL in `src/api/axios.js`:

```js
export const API_BASE_URL = 'http://localhost:5000/api'
```

## Routes

- `/login` — Sign in (unauthenticated only, redirects logged-in users to `/users`)
- `/register` — Create account (unauthenticated only)
- `/check-email` — Shown after registration, prompts user to verify via email
- `/verify/:token` — Calls `GET /api/Users/verify/{token}`, shows success/failure
- `/users` — Protected user management table (requires login)

## Features

- **Auth**: JWT stored in `localStorage`, attached to every request via Axios interceptor.
- **Auto-logout on 401**: any API response with status 401 clears the session and redirects to `/login` immediately.
- **Users table**:
  - Checkbox column for multi-select (header checkbox supports select-all / indeterminate state)
  - Persistent toolbar above the table:
    - **Block** (text button) — blocks selected, non-blocked users
    - **Unblock** (icon) — unblocks selected, blocked users
    - **Delete** (icon) — deletes selected users (with confirmation)
    - **Delete unverified** (icon) — deletes all unverified accounts (with confirmation)
  - Toolbar buttons stay visible at all times; only their disabled state changes based on selection
  - Filter input to search by name/email
  - No per-row action buttons
