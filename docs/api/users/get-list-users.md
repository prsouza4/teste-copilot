# GET /api/users

> Retrieves a paginated list of users from the registration system.

**Status:** `stable`  
**Authentication:** Not required  
**Roles:** N/A

---

## Request

### Headers

| Header | Value | Required |
|--------|-------|----------|
| `Accept` | `application/json` | No |

### Query Parameters

| Parameter | Type | Required | Default | Constraints | Description |
|-----------|------|----------|---------|-------------|-------------|
| `page` | `int` | No | `1` | Min: 1 | Page number (1-based indexing) |
| `pageSize` | `int` | No | `10` | Min: 1, Max: 100 | Number of users per page |

**Notes:**
- Page numbering starts at 1 (not 0)
- If `page` exceeds the total number of pages, an empty `items` array is returned
- Maximum `pageSize` is typically 100 (enforced by application logic)

---

## Responses

### 200 OK

Returns a paginated list of users.

```json
{
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "name": "João Silva",
      "email": "joao@example.com",
      "createdAt": "2024-01-15T10:30:00Z",
      "isActive": true
    },
    {
      "id": "8d7b2e1a-4f3c-4b2a-9e1d-6c5b4a3d2e1f",
      "name": "Maria Santos",
      "email": "maria@example.com",
      "createdAt": "2024-01-15T11:45:00Z",
      "isActive": true
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 42,
  "totalPages": 5
}
```

| Field | Type | Description |
|-------|------|-------------|
| `items` | `array` | Array of user objects (see UserDto structure below) |
| `page` | `int` | Current page number |
| `pageSize` | `int` | Number of items per page |
| `totalCount` | `int` | Total number of users in the system |
| `totalPages` | `int` | Total number of pages available |

**UserDto structure:**

| Field | Type | Description |
|-------|------|-------------|
| `id` | `string` (Guid) | User's unique identifier |
| `name` | `string` | User's full name |
| `email` | `string` | User's email address (lowercase) |
| `createdAt` | `string` (ISO 8601) | UTC timestamp when user was created |
| `isActive` | `boolean` | Whether the user account is active |

### 400 Bad Request

Invalid query parameters (e.g., negative page number, invalid format).

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "page": ["The value '-1' is not valid for page."],
    "pageSize": ["The field pageSize must be between 1 and 100."]
  }
}
```

---

## Example: curl

```bash
# Default pagination (page 1, 10 items)
curl -X GET https://api.example.com/api/users \
  -H "Accept: application/json"

# Custom pagination
curl -X GET "https://api.example.com/api/users?page=2&pageSize=20" \
  -H "Accept: application/json"
```

**Successful response:**
```bash
HTTP/1.1 200 OK
Content-Type: application/json

{
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "name": "João Silva",
      "email": "joao@example.com",
      "createdAt": "2024-01-15T10:30:00Z",
      "isActive": true
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 42,
  "totalPages": 5
}
```

## Example: fetch (JavaScript)

```javascript
// Basic usage with default pagination
const response = await fetch('/api/users')

if (!response.ok) {
  throw new Error('Failed to fetch users')
}

const data = await response.json()
console.log(`Showing ${data.items.length} of ${data.totalCount} users`)
console.log(`Page ${data.page} of ${data.totalPages}`)

// Custom pagination
async function fetchUsers(page = 1, pageSize = 20) {
  const params = new URLSearchParams({
    page: page.toString(),
    pageSize: pageSize.toString()
  })

  const response = await fetch(`/api/users?${params}`)

  if (!response.ok) {
    throw new Error('Failed to fetch users')
  }

  return response.json()
}

// Usage
const result = await fetchUsers(2, 20)
result.items.forEach(user => {
  console.log(`${user.name} (${user.email})`)
})
```

## Example: TypeScript types

```typescript
// Response
interface UserDto {
  id: string
  name: string
  email: string
  createdAt: string
  isActive: boolean
}

interface PaginatedResponse<T> {
  items: T[]
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
}

type UserListResponse = PaginatedResponse<UserDto>

// Usage
interface FetchUsersParams {
  page?: number
  pageSize?: number
}

async function fetchUsers(params: FetchUsersParams = {}): Promise<UserListResponse> {
  const { page = 1, pageSize = 10 } = params

  const searchParams = new URLSearchParams({
    page: page.toString(),
    pageSize: pageSize.toString()
  })

  const response = await fetch(`/api/users?${searchParams}`)

  if (!response.ok) {
    throw new Error(`HTTP ${response.status}: ${response.statusText}`)
  }

  return response.json()
}

// Advanced example: Fetch all pages
async function fetchAllUsers(): Promise<UserDto[]> {
  const allUsers: UserDto[] = []
  let currentPage = 1
  let totalPages = 1

  do {
    const response = await fetchUsers({ page: currentPage, pageSize: 100 })
    allUsers.push(...response.items)
    totalPages = response.totalPages
    currentPage++
  } while (currentPage <= totalPages)

  return allUsers
}

// React hook example
function useUsers(page: number, pageSize: number) {
  const [data, setData] = React.useState<UserListResponse | null>(null)
  const [loading, setLoading] = React.useState(true)
  const [error, setError] = React.useState<Error | null>(null)

  React.useEffect(() => {
    setLoading(true)
    fetchUsers({ page, pageSize })
      .then(setData)
      .catch(setError)
      .finally(() => setLoading(false))
  }, [page, pageSize])

  return { data, loading, error }
}
```

---

## Backend Reference

| Item | Value |
|------|-------|
| Controller | `UsersController` |
| Action | `ListUsers` |
| Query | `ListUsersQuery` |
| Handler | `ListUsersQueryHandler` |
| Response DTO | `PaginatedResponse<UserDto>` |
| File | `src/Cadastro/Cadastro.API/Controllers/UsersController.cs` |

---

## Business Logic Notes

- Users are returned in creation order (oldest first) by default
- Pagination is handled at the database level for performance
- Empty result sets return valid pagination metadata with empty `items` array
- Total count is calculated efficiently using database COUNT queries
- No filtering or search capabilities in the current version
- All active and inactive users are included in results
- Response time scales with page number for very large datasets
