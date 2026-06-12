#  Task Management Application

A full-stack Task Management Application built with **ASP.NET Core 8**, **PostgreSQL**, and **Next.js**.

The application provides secure user authentication, task management, filtering, searching, sorting, pagination, and authorization.

---

#  Live Demo

## Frontend

https://taskmanagement-f.vercel.app/

## Backend API (Swagger)

https://taskmanagement-1h2n.onrender.com/swagger

---

#  Features

##  Authentication & Authorization

* User Registration
* User Login
* JWT Authentication
* Password Hashing using BCrypt
* Protected API Endpoints

---

##  Task Management

* Create Tasks
* View Tasks
* Update Tasks
* Delete Tasks
* Mark Tasks as Completed
* Task Status Tracking
* Task Priority Management
* Due Date Management

---

##  Search, Filter & Sort

### Search

* Search Tasks by Title

### Filter

* Filter Tasks by Status

### Sort

* Sort by Created Date
* Sort by Due Date
* Sort by Priority


---

##  Pagination

* Server-Side Pagination
* Configurable Page Size
* Total Records Count
* Total Pages Count

---

##  Validation & Error Handling

* FluentValidation
* Global Exception Handling Middleware
* Consistent API Response Structure
* Proper HTTP Status Codes

---

#  Tech Stack

## Frontend

* Next.js
* TypeScript
* Tailwind CSS

## Backend

* ASP.NET Core 8 Web API

## Database

* PostgreSQL

---

#  Deployment

| Service  | Platform        |
| -------- | --------------- |
| Frontend | Vercel          |
| Backend  | Render          |
| Database | Neon PostgreSQL |

---

#  Project Setup

## Clone Repository

```bash
git clone https://github.com/Sridhar311/Taskmanagementt.git

cd Taskmanagementt
```

---

#  Backend Setup

Navigate to API project:

```bash
cd TaskManagement.Api
```

Restore dependencies:

```bash
dotnet restore
```

Apply migrations:

```bash
dotnet ef database update
```

Run API:

```bash
dotnet run
```

Swagger available at:

```text
https://localhost:5001/swagger
```

---

#  Frontend Setup

Navigate to frontend:

```bash
cd task-ui
```

Install dependencies:

```bash
npm install
```

Run development server:

```bash
npm run dev
```

Frontend available at:

```text
http://localhost:3000
```

---

#  API Features

### Authentication

* Register User
* Login User

### Task Operations

* Create Task
* Get Tasks
* Get Task By ID
* Update Task
* Delete Task

### Other Features

* Search
* Filter
* Sort
* Pagination
* JWT Authorization

---

#  Project Highlights

✅ Secure JWT Authentication

✅ User-Specific Task Authorization

✅ CRUD Operations

✅ Search, Filter & Sort

✅ Server-Side Pagination

✅ PostgreSQL Integration

✅ FluentValidation

✅ Global Exception Handling

✅ Fully Responsive UI

✅ Full-Stack Deployment
