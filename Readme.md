# Task Manager API

A simple **Task Management REST API** built with **C# .NET Web API**, **EF Core**, and **SQLite**.  
Features include **JWT authentication**, **validation**, **pagination**, **filters**, and **Swagger documentation**.

---

## Features

- **User Management**: Create and list users with validation (name, email, password).  
- **Task Management**: CRUD tasks with filters (status, priority, due date, assigned user) and pagination.  
- **JWT Authentication**: Secure login and protected endpoints.  
- **Validation & Error Handling**: Proper HTTP status codes (400, 404, 409, 201).  
- **Swagger Documentation**: Interactive API docs at `/swagger/index.html`.  

---

## Live Hosted API

- **Base URL:** [https://taskmanager-29ar.onrender.com](https://taskmanager-29ar.onrender.com)  
- **Swagger Docs:** [https://taskmanager-29ar.onrender.com/swagger/index.html](https://taskmanager-29ar.onrender.com/swagger/index.html)

> Use JWT token for authorization in Swagger (click **Authorize**) or via `Authorization: Bearer <token>` header.

---

## Local Setup & Docker

### **Prerequisites**

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)  
- SQLite (optional, included via NuGet)  
- Git  
- Docker (optional, for containerized run)  

---

### **Clone the Repository**

```bash
git clone https://github.com/Chibhiraj/TaskManager.git
cd TaskManager



 --------------------------------Local Setup ----------------------------------

Install EF Core CLI (if not installed):

dotnet tool install --global dotnet-ef


Apply database migrations to create SQLite database:

dotnet ef database update


Run the API locally:

dotnet run


The API will be available at https://localhost:5001 or http://localhost:5000.

Swagger docs: https://localhost:5001/swagger/index.html


-------------------------------------------Docker Setup-------------------------------------

Build the Docker image:

docker build -t taskmanagerapi .


Run the container (port mapping 5000):

docker run -p 5000:5000 taskmanagerapi


Access the API inside Docker:

Base URL: http://localhost:5000

Swagger docs: http://localhost:5000/swagger/index.html