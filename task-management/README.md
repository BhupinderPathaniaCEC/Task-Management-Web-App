# Task Management System 🚀

A full-stack task management application built with .NET 8, Angular, and SQLite.

## 📋 Table of Contents

- [Project Overview](#project-overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Environment Setup](#environment-setup)
- [Quick Start](#quick-start)
- [Project Structure](#project-structure)
- [Documentation](#documentation)
- [Development](#development)
- [Railway Deployment](#railway-deployment)
- [Troubleshooting](#troubleshooting)

## 🎯 Project Overview

This is a comprehensive task management web application that allows users to:
- Create and manage projects
- Add team members to projects
- Create and track tasks
- Update task statuses (Todo → In Progress → Done)
- Role-based access control (Admin/Member)

## ✨ Features

- **User Authentication**: JWT-based secure authentication
- **Project Management**: Create projects and manage team members
- **Task Tracking**: Create, update, and track task progress
- **Role-Based Access**: Admin and Member roles with different permissions
- **Responsive UI**: Modern Angular frontend with Tailwind CSS
- **Real-time Updates**: Reactive state management with Angular Signals
- **Database**: SQLite with Entity Framework Core

## 🛠️ Technology Stack

### Backend
- **.NET 8** - Web API framework
- **ASP.NET Core Identity** - User authentication & authorization
- **Entity Framework Core** - ORM for database operations
- **SQLite** - Lightweight database
- **JWT (JSON Web Tokens)** - Token-based authentication
- **Swagger/OpenAPI** - API documentation

### Frontend
- **Angular 18** - Frontend framework
- **TypeScript** - Type-safe JavaScript
- **Tailwind CSS** - Utility-first CSS framework
- **Angular Signals** - Reactive state management
- **RxJS** - Reactive programming

## 🔧 Environment Setup

### Prerequisites

Before starting, ensure you have the following installed:

1. **.NET 8 SDK**
   ```bash
   # Download from: https://dotnet.microsoft.com/download/dotnet/8.0
   # Verify installation:
   dotnet --version
   ```

2. **Node.js 18+**
   ```bash
   # Download from: https://nodejs.org/
   # Verify installation:
   node --version
   npm --version
   ```

3. **Angular CLI**
   ```bash
   npm install -g @angular/cli
   # Verify installation:
   ng version
   ```

4. **Git** (for version control)
   ```bash
   # Download from: https://git-scm.com/downloads
   # Verify installation:
   git --version
   ```

5. **Visual Studio Code** (recommended IDE)
   ```bash
   # Download from: https://code.visualstudio.com/
   ```

### Recommended VS Code Extensions

- C# Dev Kit
- .NET Install Tool
- Angular Language Service
- Tailwind CSS IntelliSense
- Prettier - Code formatter
- ESLint

## 🚀 Quick Start

### 1. Clone the Repository

```bash
git clone <repository-url>
cd To-Do-Web-App/task-management
```

### 2. Backend Setup

Navigate to the backend directory and set up the API:

```bash
cd src/TaskManagement.Api

# Restore dependencies
dotnet restore

# Run the API
dotnet run
```

The backend will start at:
- **API URL**: http://localhost:5130
- **Swagger UI**: http://localhost:5130/swagger

### 3. Frontend Setup

Open a new terminal and navigate to the frontend directory:

```bash
cd ui/taskmanagement-ui

# Install dependencies
npm install

# Start the development server
ng serve
```

The frontend will start at:
- **Application URL**: http://localhost:4200

### 4. Access the Application

1. Open your browser and go to: http://localhost:4200
2. Register a new account (first user will be Admin)
3. Create a project
4. Add tasks and start managing!

## 📁 Project Structure

```
task-management/
├── src/                          # Backend (.NET)
│   ├── TaskManagement.Api/       # Web API project
│   ├── TaskManagement.Application/ # Business logic
│   ├── TaskManagement.Domain/    # Domain entities & enums
│   ├── TaskManagement.Infrastructure/ # Data access (EF Core)
│   └── README.md                 # Detailed API documentation
│
├── ui/                           # Frontend (Angular)
│   └── taskmanagement-ui/        # Angular application
│       ├── src/
│       │   ├── app/
│       │   │   ├── services/     # API services
│       │   │   ├── pages/        # Page components
│       │   │   ├── guards/       # Route guards
│       │   │   └── models/       # TypeScript interfaces
│       │   └── assets/           # Static assets
│       ├── angular.json          # Angular configuration
│       └── package.json          # Node dependencies
│
├── .gitignore                    # Git ignore rules
└── README.md                     # This file
```

## 📚 Documentation

### Backend API Documentation

For detailed API documentation including:
- All endpoints and their methods
- Request/response formats
- Database schema
- Authentication details
- Error handling

**See**: [src/README.md](./src/README.md)

### API Endpoints Summary

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/auth/register` | POST | Register new user |
| `/api/auth/login` | POST | Authenticate user |
| `/api/projects` | GET | Get user projects |
| `/api/projects` | POST | Create project |
| `/api/projects/{id}/members` | POST | Add project member |
| `/api/tasks/project/{id}` | GET | Get project tasks |
| `/api/tasks` | POST | Create task |
| `/api/tasks/{id}/status` | PATCH | Update task status |

## 💻 Development

### Running Both Services Simultaneously

**Option 1: Using two terminals**
```bash
# Terminal 1 - Backend
cd src/TaskManagement.Api
dotnet run

# Terminal 2 - Frontend
cd ui/taskmanagement-ui
ng serve
```

**Option 2: Using VS Code**
- Open the project in VS Code
- Open integrated terminal (Ctrl + `)
- Use terminal split to run both services

### Building for Production

**Backend:**
```bash
cd src/TaskManagement.Api
dotnet build --configuration Release
dotnet publish --configuration Release --output ./publish
```

**Frontend:**
```bash
cd ui/taskmanagement-ui
ng build --configuration production
```

### Database Management

The SQLite database (`TaskManagement.db`) is automatically created on first run. It's located in the backend project root.

To reset the database:
```bash
# Delete the database file
cd src/TaskManagement.Api
rm TaskManagement.db
# Restart the API to recreate it
dotnet run
```

## 🔐 Authentication

The application uses JWT (JSON Web Tokens) for authentication:

1. **Registration**: Create an account with email and password
2. **Login**: Receive a JWT access token
3. **Authorization**: Include token in API requests
4. **Token Storage**: Stored in browser localStorage

**Default Roles:**
- **Admin**: Full access to all features (first registered user)
- **Member**: Can create projects/tasks, update assigned tasks

## � Railway Deployment

This project can be deployed to Railway for production hosting with automatic HTTPS and a live URL.

### Prerequisites

1. **Railway Account**: Sign up at [railway.app](https://railway.app)
2. **GitHub Repository**: Push your code to a GitHub repository
3. **Railway CLI** (optional): Install for command-line deployment

### Deploy Backend (.NET API)

1. **Create a new Railway project:**
   - Go to [railway.app](https://railway.app) and click "New Project"
   - Select "Deploy from GitHub repo"
   - Choose your repository
   - Select the `src/TaskManagement.Api` directory as the root

2. **Configure environment variables:**
   - Go to your Railway project → Variables tab
   - Add the following variables:
     ```
     Jwt__Issuer=TaskManagement
     Jwt__Audience=TaskManagement
     Jwt__SigningKey=YOUR_SECURE_256_BIT_KEY_HERE
     AllowedOrigins=https://your-frontend-url.railway.app
     ```

3. **Set up persistent storage:**
   - Go to your Railway project → Settings → Volumes
   - Add a volume at path `/app/data` to persist SQLite database

4. **Deploy:**
   - Railway will automatically build and deploy using the Dockerfile
   - Wait for the deployment to complete
   - Copy the generated Railway URL (e.g., `https://your-api.railway.app`)

### Deploy Frontend (Angular)

1. **Create a new Railway project:**
   - Click "New Project" in Railway
   - Select "Deploy from GitHub repo"
   - Choose your repository
   - Select the `ui/taskmanagement-ui` directory as the root

2. **Configure environment variables:**
   - Go to your Railway project → Variables tab
   - Add the following variable:
     ```
     RAILWAY_PUBLIC_URL=https://your-api-railway-app-url.railway.app
     ```
   - Replace with your actual backend Railway URL from the previous step

3. **Deploy:**
   - Railway will automatically build and deploy using the Dockerfile
   - Wait for the deployment to complete
   - Copy the generated Railway URL (e.g., `https://your-frontend.railway.app`)

### Update Backend CORS

After deploying both services:

1. Go to your **Backend Railway project** → Variables tab
2. Update the `AllowedOrigins` variable:
   ```
   AllowedOrigins=https://your-frontend-railway-app-url.railway.app
   ```
3. Redeploy the backend service for changes to take effect

### Access Your Live Application

- **Frontend URL**: `https://your-frontend.railway.app`
- **Backend API**: `https://your-api.railway.app`
- **API Documentation**: `https://your-api.railway.app/swagger`

### Railway Configuration Files

The project includes the following configuration files for Railway:

**Backend:**
- `src/TaskManagement.Api/railway.json` - Railway build configuration
- `src/TaskManagement.Api/Dockerfile` - Docker build instructions
- `src/TaskManagement.Api/.dockerignore` - Files to exclude from Docker build

**Frontend:**
- `ui/taskmanagement-ui/railway.json` - Railway build configuration
- `ui/taskmanagement-ui/Dockerfile` - Docker build instructions
- `ui/taskmanagement-ui/nginx.conf` - Nginx configuration for serving static files
- `ui/taskmanagement-ui/.dockerignore` - Files to exclude from Docker build

### Environment Variables Reference

| Variable | Backend | Frontend | Description |
|----------|---------|----------|-------------|
| `Jwt__Issuer` | ✅ | ❌ | JWT token issuer |
| `Jwt__Audience` | ✅ | ❌ | JWT token audience |
| `Jwt__SigningKey` | ✅ | ❌ | JWT signing secret key |
| `AllowedOrigins` | ✅ | ❌ | CORS allowed origins (comma-separated) |
| `RAILWAY_PUBLIC_URL` | ❌ | ✅ | Backend API URL for frontend |

### Monitoring & Logs

1. **View logs:**
   - Go to Railway project → Logs tab
   - View real-time application logs

2. **Monitor deployments:**
   - Go to Railway project → Deployments tab
   - View deployment history and status

3. **Set up alerts:**
   - Go to Railway project → Settings → Notifications
   - Configure alerts for deployment failures or errors

### Troubleshooting Railway Deployment

**Deployment fails:**
- Check the build logs in Railway
- Ensure all files are committed to GitHub
- Verify Dockerfile syntax is correct

**CORS errors:**
- Ensure `AllowedOrigins` includes the frontend Railway URL
- Check that both services are running
- Verify environment variables are set correctly

**Database errors:**
- Ensure persistent storage volume is configured at `/app/data`
- Check that SQLite database is being created in the correct location

**Frontend can't connect to API:**
- Verify `RAILWAY_PUBLIC_URL` is set correctly in frontend
- Check backend CORS configuration
- Ensure backend is deployed and accessible

### Cost Considerations

Railway offers a free tier with:
- $5/month free credit
- Up to 512MB RAM
- Shared CPU
- 1GB storage

For production use, consider upgrading to a paid plan for:
- More RAM and CPU
- Dedicated resources
- Higher storage limits

---

## �🐛 Troubleshooting

### Backend Issues

**Port already in use:**
```bash
# Find process using port 5130 (Windows)
netstat -ano | findstr :5130
taskkill /PID <PID> /F

# (Linux/Mac)
lsof -ti:5130 | xargs kill -9
```

**Database errors:**
- Delete `TaskManagement.db` and restart the API
- Ensure EF Core migrations are applied

### Frontend Issues

**Port already in use:**
```bash
# Use a different port
ng serve --port 4201
```

**Module not found errors:**
```bash
# Clear Angular cache
rm -rf node_modules/.angular
npm install
```

**CORS errors:**
- Ensure backend is running
- Check CORS configuration in `Program.cs`

### Common Issues

1. **401 Unauthorized**: Check if token is valid and not expired
2. **403 Forbidden**: Verify user has required permissions
3. **500 Internal Server Error**: Check backend logs for detailed error
4. **CORS errors**: Ensure both services are running on correct ports

## 📝 Environment Variables

### Backend (appsettings.json)

```json
{
  "Jwt": {
    "Issuer": "TaskManagement",
    "Audience": "TaskManagement",
    "SigningKey": "your-256-bit-secret-key-here"
  }
}
```

### Frontend (environment.ts)

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5130/api'
};
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License.

## 📞 Support

For issues and questions:
- Check the [Troubleshooting](#troubleshooting) section
- Review the [API Documentation](./src/README.md)
- Open an issue on GitHub

---

**Built with ❤️ using .NET 8 and Angular 18**