# GET /api/users/{id}

> Retrieves a single user by their unique identifier.

**Status:** `stable`  
**Authentication:** Not required  
**Roles:** N/A

---

## Request

### Headers

| Header | Value | Required |
|--------|-------|----------|
| `Accept` | `application/json` | No |

### Route Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | `Guid` | Yes | User's unique identifier (UUID format) |

**Constraints:**
- `id` must be a valid GUID/UUID format (e.g., `3fa85f64-5717-4562-b3fc-2c963f66afa6`)

---

## Responses

### 200 OK

User found and returned successfully.

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "João Silva",
  "email": "joao@example.com",
  "createdAt": "2024-01-15T10:30:00Z",
  "isActive": true
}
```

| Field | Type | Description |
|-------|------|-------------|
| `id` | `string` (Guid) | User's unique identifier |
| `name` | `string` | User's full name |
| `email` | `string` | User's email address (lowercase) |
| `createdAt` | `string` (ISO 8601) | UTC timestamp when user was created |
| `isActive` | `boolean` | Whether the user account is active |

### 404 Not Found

User with the specified ID does not exist.

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

### 400 Bad Request

Invalid GUID format provided.

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "id": ["The value 'invalid-id' is not valid."]
  }
}
```

---

## Example: curl

```bash
curl -X GET https://api.example.com/api/users/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Accept: application/json"
```

**Successful response:**
```bash
HTTP/1.1 200 OK
Content-Type: application/json

{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "João Silva",
  "email": "joao@example.com",
  "createdAt": "2024-01-15T10:30:00Z",
  "isActive": true
}
```

**Not found response:**
```bash
HTTP/1.1 404 Not Found
```

## Example: fetch (JavaScript)

```javascript
const userId = '3fa85f64-5717-4562-b3fc-2c963f66afa6'

const response = await fetch(`/api/users/${userId}`, {
  method: 'GET',
  headers: { 'Accept': 'application/json' }
})

if (response.status === 404) {
  console.error('User not found')
  return null
}

if (!response.ok) {
  throw new Error('Failed to fetch user')
}

const user = await response.json()
console.log('User:', user)
console.log('Active:', user.isActive)
```

## Example: TypeScript types

```typescript
// Response
interface UserDto {
  id: string
  name: string
  email: string
  createdAt: string  // ISO 8601 date string
  isActive: boolean
}

// Usage
async function getUserById(userId: string): Promise<UserDto | null> {
  const response = await fetch(`/api/users/${userId}`)

  if (response.status === 404) {
    return null
  }

  if (!response.ok) {
    throw new Error(`Failed to fetch user: ${response.statusText}`)
  }

  return response.json()
}

// Example with error handling
async function getUserWithErrorHandling(userId: string): Promise<UserDto> {
  try {
    const response = await fetch(`/api/users/${userId}`)

    if (response.status === 404) {
      throw new Error('User not found')
    }

    if (!response.ok) {
      const error = await response.json()
      throw new Error(error.title || 'Unknown error')
    }

    const user: UserDto = await response.json()
    
    // Parse date if needed
    const createdDate = new Date(user.createdAt)
    console.log('User created:', createdDate.toLocaleDateString())

    return user
  } catch (error) {
    console.error('Error fetching user:', error)
    throw error
  }
}
```

---

## Backend Reference

| Item | Value |
|------|-------|
| Controller | `UsersController` |
| Action | `GetUserById` |
| Query | `GetUserByIdQuery` |
| Handler | `GetUserByIdQueryHandler` |
| Response DTO | `UserDto` |
| File | `src/Cadastro/Cadastro.API/Controllers/UsersController.cs` |

---

## Business Logic Notes

- Returns `null` from handler when user is not found, mapped to 404 by controller
- Query reads from the user repository without modification
- No caching implemented at application layer
- Email is returned in lowercase (normalized during creation)
- `createdAt` is always in UTC timezone
- `isActive` reflects current account status (can be deactivated by administrators)
