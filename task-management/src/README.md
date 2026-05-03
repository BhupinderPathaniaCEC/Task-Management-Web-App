# Task Management API 🚀

A comprehensive task management system built with .NET 8, Angular, and SQLite.

## 📋 Table of Contents

- [Architecture](#architecture)
- [Database Schema](#database-schema)
- [API Endpoints](#api-endpoints)
- [Setup Instructions](#setup-instructions)
- [Authentication](#authentication)

## 🏗️ Architecture

```
├── TaskManagement.Api (ASP.NET Core Web API)
├── TaskManagement.Application (Business Logic)
├── TaskManagement.Domain (Entities & Enums)
├── TaskManagement.Infrastructure (Data Access)
└── TaskManagement.UI (Angular Frontend)
```

## 🗄️ Database Schema

### Users Table (ASP.NET Identity)

| Column | Type | Description |
|--------|------|-------------|
| Id | string | Primary Key (GUID) |
| UserName | string | Unique username |
| Email | string | User email |
| PasswordHash | string | Hashed password |
| Role | string | User role (Admin/Member) |

### Projects Table

| Column | Type | Description |
|--------|------|-------------|
| Id | Guid | Primary Key |
| Name | string | Project name (max 200 chars) |
| Description | string | Project description |
| CreatedByUserId | string | Creator's user ID |
| CreatedAt | DateTimeOffset | Creation timestamp |

### ProjectMembers Table (Junction)

| Column | Type | Description |
|--------|------|-------------|
| ProjectId | Guid | Foreign Key → Projects.Id |
| UserId | string | Foreign Key → Users.Id |

**Primary Key:** ProjectId + UserId

### Tasks Table

| Column | Type | Description |
|--------|------|-------------|
| Id | Guid | Primary Key |
| ProjectId | Guid | Foreign Key → Projects.Id |
| Title | string | Task title (max 200 chars) |
| Description | string | Task description |
| Status | int | Task status (0=Todo, 1=InProgress, 2=Done) |
| DueDate | DateOnly | Task due date |
| AssigneeUserId | string | Foreign Key → Users.Id |
| CreatedAt | DateTimeOffset | Creation timestamp |

### TaskStatus Enum

```csharp
public enum TaskStatus
{
    Todo = 0,
    InProgress = 1,
    Done = 2
}
```

### Relationships

```
Users (1) ──── (Many) ProjectMembers (Many) ──── (1) Projects
                               │
                               │
                               └─── (Many) Tasks

Projects (1) ──── (Many) Tasks
Users (1) ──── (Many) Tasks (Assignee)
```

## 🔗 API Endpoints

### Authentication Endpoints

#### POST /api/auth/register

Register a new user.

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "role": "Admin" | "Member"
}
```

**Response (200 OK):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs..."
}
```

**Error Responses:**
- `400 Bad Request` - Invalid email/password

#### POST /api/auth/login

Authenticate user.

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

**Response (200 OK):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs..."
}
```

**Error Responses:**
- `401 Unauthorized` - Invalid credentials

### Project Endpoints

#### GET /api/projects

Get all projects for the authenticated user.

**Headers:**
```
Authorization: Bearer {accessToken}
```

**Response (200 OK):**
```json
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "name": "Project Alpha",
    "description": "First project",
    "createdAt": "2024-01-01T10:00:00Z"
  }
]
```

**Error Responses:**
- `401 Unauthorized` - Invalid token

#### POST /api/projects

Create a new project.

**Headers:**
```
Authorization: Bearer {accessToken}
```

**Request Body:**
```json
{
  "name": "New Project",
  "description": "Project description"
}
```

**Response (200 OK):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "New Project",
  "description": "Project description",
  "createdAt": "2024-01-01T10:00:00Z"
}
```

**Error Responses:**
- `400 Bad Request` - Missing name
- `401 Unauthorized` - Invalid token
- `403 Forbidden` - Admin-only in some versions

#### POST /api/projects/{projectId}/members

Add a member to a project (Admin only).

**Headers:**
```
Authorization: Bearer {accessToken}
```

**Request Body:**
```json
{
  "userId": "c783a7f2-14a7-452f-8610-10f5a9c16c68"
}
```

**Response (204 No Content):**

**Error Responses:**
- `400 Bad Request` - Invalid userId
- `401 Unauthorized` - Invalid token
- `403 Forbidden` - Admin-only

### Task Endpoints

#### GET /api/tasks/project/{projectId}

Get all tasks for a specific project.

**Headers:**
```
Authorization: Bearer {accessToken}
```

**Response (200 OK):**
```json
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440001",
    "projectId": "550e8400-e29b-41d4-a716-446655440000",
    "title": "Complete feature",
    "description": "Implement user login",
    "status": 0,
    "dueDate": "2024-01-15",
    "assigneeUserId": "c783a7f2-14a7-452f-8610-10f5a9c16c68",
    "createdAt": "2024-01-01T10:00:00Z"
  }
]
```

**Error Responses:**
- `401 Unauthorized` - Invalid token
- `404 Not Found` - Project not found

#### POST /api/tasks

Create a new task.

**Headers:**
```
Authorization: Bearer {accessToken}
```

**Request Body:**
```json
{
  "projectId": "550e8400-e29b-41d4-a716-446655440000",
  "title": "New Task",
  "description": "Task description",
  "assigneeUserId": "c783a7f2-14a7-452f-8610-10f5a9c16c68",
  "dueDate": "2024-01-15"
}
```

**Response (200 OK):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440001",
  "projectId": "550e8400-e29b-41d4-a716-446655440000",
  "title": "New Task",
  "description": "Task description",
  "status": 0,
  "dueDate": "2024-01-15",
  "assigneeUserId": "c783a7f2-14a7-452f-8610-10f5a9c16c68",
  "createdAt": "2024-01-01T10:00:00Z"
}
```

**Error Responses:**
- `400 Bad Request` - Missing title or invalid project
- `401 Unauthorized` - Invalid token

#### PATCH /api/tasks/{taskId}/status

Update task status.

**Headers:**
```
Authorization: Bearer {accessToken}
```

**Request Body:**
```json
{
  "status": 1
}
```

**Response (204 No Content):**

**Error Responses:**
- `401 Unauthorized` - Invalid token
- `403 Forbidden` - Not assigned to task
- `404 Not Found` - Task not found

## 🚀 Setup Instructions

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- Angular CLI

### Backend Setup

1. **Navigate to API directory:**
   ```bash
   cd task-management/src/TaskManagement.Api
   ```

2. **Install dependencies:**
   ```bash
   dotnet restore
   ```

3. **Run the API:**
   ```bash
   dotnet run
   ```

4. **API will be available at:**
   - HTTP: http://localhost:5130
   - Swagger UI: http://localhost:5130/swagger

### Frontend Setup

1. **Navigate to UI directory:**
   ```bash
   cd task-management/ui/taskmanagement-ui
   ```

2. **Install dependencies:**
   ```bash
   npm install
   ```

3. **Start development server:**
   ```bash
   ng serve
   ```

4. **Open in browser:**
   - URL: http://localhost:4200

## 🔐 Authentication

The API uses JWT (JSON Web Tokens) for authentication.

### JWT Payload

```json
{
  "sub": "user-id-guid",
  "email": "user@example.com",
  "nameid": "user-id-guid",
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "Admin",
  "exp": 1737752589,
  "iss": "TaskManagement",
  "aud": "TaskManagement"
}
```

### Authorization Headers

Include the JWT token in the Authorization header:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

### User Roles

- **Admin**: Full access to all features
- **Member**: Can create projects/tasks, update assigned tasks

## 🛠️ Development

### Database

- **Type:** SQLite
- **File:** `TaskManagement.db`
- **Auto-created:** Yes (on first run)
- **Migrations:** Not required (EF Core auto-creates schema)

### Environment Variables

Configure in `appsettings.json`:
```json
{
  "Jwt": {
    "Issuer": "TaskManagement",
    "Audience": "TaskManagement",
    "SigningKey": "your-256-bit-secret-key-here"
  }
}
```

## 📊 Status Codes

| Code | Meaning |
|------|---------|
| 200 | OK - Success |
| 204 | No Content - Success (no response body) |
| 400 | Bad Request - Invalid input |
| 401 | Unauthorized - Invalid/missing token |
| 403 | Forbidden - Insufficient permissions |
| 404 | Not Found - Resource doesn't exist |
| 500 | Internal Server Error - Server error |

## 🎯 API Features

- ✅ User registration and authentication
- ✅ JWT-based authorization
- ✅ Project management
- ✅ Task creation and status updates
- ✅ Role-based access control
- ✅ CORS enabled for Angular frontend
- ✅ SQLite database with EF Core
- ✅ Swagger/OpenAPI documentation

---

**Note:** This API is designed for the Task Management Angular frontend. All endpoints require authentication except registration and login.