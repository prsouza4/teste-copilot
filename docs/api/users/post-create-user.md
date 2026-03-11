# POST /api/users

> Creates a new user in the registration system.

**Status:** `stable`  
**Authentication:** Not required  
**Roles:** N/A

---

## Request

### Headers

| Header | Value | Required |
|--------|-------|----------|
| `Content-Type` | `application/json` | Yes |

### Body

```json
{
  "name": "João Silva",
  "email": "joao@example.com"
}
```

| Field | Type | Required | Validation |
|-------|------|----------|------------|
| `name` | `string` | Yes | Cannot be empty, max 100 characters, trimmed |
| `email` | `string` | Yes | Cannot be empty, valid email format, normalized to lowercase |

**Validation Rules:**
- **Name:** Must not be empty or whitespace. Automatically trimmed. Cannot exceed 100 characters.
- **Email:** Must not be empty. Must match regex pattern `^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$`. Automatically converted to lowercase.
- **Email Uniqueness:** Email must not already exist in the system (enforced by domain logic).

---

## Responses

### 201 Created

User created successfully. Returns the ID of the new user and a `Location` header pointing to the user resource.

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

**Headers:**
```
Location: /api/users/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

### 400 Bad Request

Invalid request data or domain rule violation (e.g., validation failed, email already exists).

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Name": ["Name cannot be empty."],
    "Email": ["Email format is invalid."]
  }
}
```

**Common error messages:**
- `"Name cannot be empty."`
- `"Name cannot exceed 100 characters."`
- `"Email cannot be empty."`
- `"Email format is invalid."`
- `"A user with email '{email}' already exists."`

---

## Example: curl

```bash
curl -X POST https://api.example.com/api/users \
  -H "Content-Type: application/json" \
  -d '{
    "name": "João Silva",
    "email": "joao@example.com"
  }'
```

**Successful response:**
```bash
HTTP/1.1 201 Created
Location: /api/users/3fa85f64-5717-4562-b3fc-2c963f66afa6
Content-Type: application/json

{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

## Example: fetch (JavaScript)

```javascript
const response = await fetch('/api/users', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    name: 'João Silva',
    email: 'joao@example.com'
  })
})

if (!response.ok) {
  const error = await response.json()
  console.error('Error creating user:', error.title, error.errors)
  throw new Error(error.title)
}

const result = await response.json()
console.log('User created with ID:', result.id)

// Extract user URL from Location header
const userUrl = response.headers.get('Location')
console.log('User resource:', userUrl)
```

## Example: TypeScript types

```typescript
// Request
interface CreateUserRequest {
  name: string
  email: string
}

// Response
interface CreateUserResponse {
  id: string
}

// Error Response
interface ValidationError {
  type: string
  title: string
  status: number
  errors: Record<string, string[]>
}

// Usage
async function createUser(data: CreateUserRequest): Promise<CreateUserResponse> {
  const response = await fetch('/api/users', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data)
  })

  if (!response.ok) {
    const error: ValidationError = await response.json()
    throw new Error(error.title)
  }

  return response.json()
}
```

---

## Backend Reference

| Item | Value |
|------|-------|
| Controller | `UsersController` |
| Action | `CreateUser` |
| Command | `CreateUserCommand` |
| Handler | `CreateUserCommandHandler` |
| Domain Entity | `User` |
| Value Objects | `Name`, `Email` |
| Event | `UserCreatedEvent` |
| File | `src/Cadastro/Cadastro.API/Controllers/UsersController.cs` |

---

## Business Logic Notes

- User creation triggers a `UserCreatedEvent` that is published to external systems
- Email is stored in lowercase for case-insensitive comparison
- User is automatically set to active (`IsActive = true`) on creation
- Each user gets a unique GUID identifier
- Creation timestamp (`CreatedAt`) is set to UTC time
