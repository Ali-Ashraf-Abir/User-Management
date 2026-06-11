const iconBase =
  'inline-flex items-center justify-center h-9 w-9 rounded-md border transition-colors disabled:opacity-40 disabled:cursor-not-allowed'

export default function UsersToolbar({
  selectedCount,
  hasBlockable,
  hasUnblockable,
  onBlock,
  onUnblock,
  onDelete,
  onDeleteUnverified,
  filter,
  onFilterChange,
}) {
  return (
    <div className="flex flex-wrap items-center justify-between gap-3 border border-gray-200 bg-gray-50 rounded-t-lg px-4 py-2.5">
      <div className="flex items-center gap-2">
        {/* Block - button with text */}
        <button
          type="button"
          onClick={onBlock}
          disabled={selectedCount === 0 || !hasBlockable}
          title="Block selected users"
          className="inline-flex items-center gap-1.5 rounded-md border border-gray-300 bg-white px-3 py-1.5 text-sm font-medium text-gray-700 hover:bg-gray-100 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
        >
          <svg xmlns="http://www.w3.org/2000/svg" className="h-4 w-4" viewBox="0 0 20 20" fill="currentColor">
            <path
              fillRule="evenodd"
              d="M10 1a4.5 4.5 0 00-4.5 4.5V9H5a2 2 0 00-2 2v6a2 2 0 002 2h10a2 2 0 002-2v-6a2 2 0 00-2-2h-.5V5.5A4.5 4.5 0 0010 1zm3 8V5.5a3 3 0 10-6 0V9h6z"
              clipRule="evenodd"
            />
          </svg>
          Block
        </button>

        {/* Unblock - icon only */}
        <button
          type="button"
          onClick={onUnblock}
          disabled={selectedCount === 0 || !hasUnblockable}
          title="Unblock selected users"
          aria-label="Unblock selected users"
          className={`${iconBase} border-gray-300 bg-white text-gray-700 hover:bg-gray-100`}
        >
          <svg xmlns="http://www.w3.org/2000/svg" className="h-4 w-4" viewBox="0 0 20 20" fill="currentColor">
            <path d="M8.5 5.5a4.5 4.5 0 119 0V9h.5a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2h5V5.5a3 3 0 00-6 0V8a1 1 0 11-2 0V5.5A4.5 4.5 0 018.5 5.5z" />
          </svg>
        </button>

        {/* Delete selected - icon only */}
        <button
          type="button"
          onClick={onDelete}
          disabled={selectedCount === 0}
          title="Delete selected users"
          aria-label="Delete selected users"
          className={`${iconBase} border-red-200 bg-white text-red-600 hover:bg-red-50`}
        >
          <svg xmlns="http://www.w3.org/2000/svg" className="h-4 w-4" viewBox="0 0 20 20" fill="currentColor">
            <path
              fillRule="evenodd"
              d="M8.75 1A2.75 2.75 0 006 3.75v.443c-.795.077-1.584.176-2.365.298a.75.75 0 10.23 1.482l.149-.022.841 10.518A2.75 2.75 0 007.596 19h4.808a2.75 2.75 0 002.742-2.53l.841-10.519.149.023a.75.75 0 00.23-1.482A41.03 41.03 0 0014 4.193V3.75A2.75 2.75 0 0011.25 1h-2.5zM10 4c.84 0 1.673.025 2.5.075V3.75c0-.69-.56-1.25-1.25-1.25h-2.5c-.69 0-1.25.56-1.25 1.25v.325C8.327 4.025 9.16 4 10 4zM8.58 7.72a.75.75 0 00-1.5.06l.3 7.5a.75.75 0 101.5-.06l-.3-7.5zm4.34.06a.75.75 0 10-1.5-.06l-.3 7.5a.75.75 0 101.5.06l.3-7.5z"
              clipRule="evenodd"
            />
          </svg>
        </button>

        {/* Delete unverified - icon only */}
        <button
          type="button"
          onClick={onDeleteUnverified}
          title="Delete all unverified users"
          aria-label="Delete all unverified users"
          className={`${iconBase} border-orange-200 bg-white text-orange-600 hover:bg-orange-50`}
        >
          <svg xmlns="http://www.w3.org/2000/svg" className="h-4 w-4" viewBox="0 0 20 20" fill="currentColor">
            <path
              fillRule="evenodd"
              d="M9.401 3.003c1.155-2 4.043-2 5.197 0l7.355 12.748c1.154 2-.29 4.5-2.599 4.5H4.645c-2.309 0-3.752-2.5-2.598-4.5L9.4 3.003zM12 8.25a.75.75 0 01.75.75v3.75a.75.75 0 01-1.5 0V9a.75.75 0 01.75-.75zm0 8.25a.75.75 0 100-1.5.75.75 0 000 1.5z"
              clipRule="evenodd"
            />
          </svg>
        </button>
      </div>

      <div>
        <input
          type="text"
          value={filter}
          onChange={(e) => onFilterChange(e.target.value)}
          placeholder="Filter"
          className="w-48 rounded-md border border-gray-300 px-3 py-1.5 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        />
      </div>
    </div>
  )
}
