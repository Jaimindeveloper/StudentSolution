# 🎓 Student Management System – ASP.NET Core 10 Web API

A practical **Student Management System REST API** built using **ASP.NET Core 10 Web API** and **Entity Framework Core**.

This project demonstrates a real-world CRUD-based API with **search, server-side pagination, and CSV export** functionality. It is designed as a learning and portfolio project to demonstrate modern .NET backend development practices.

---

## 🚀 Features

* ✅ Create Student
* ✅ Get Student by ID
* ✅ Get All Students
* ✅ Update Student
* ✅ Delete Student
* ✅ Search Students
* ✅ Server-side Pagination
* ✅ Download Student Data as CSV
* ✅ Entity Framework Core
* ✅ SQL Server Database
* ✅ LINQ Queries
* ✅ Async/Await
* ✅ DTO-based API design
* ✅ Swagger / OpenAPI
* ✅ RESTful API architecture
* ✅ Data validation using Data Annotations

---

## 🛠️ Technology Stack

| Technology            | Version / Usage          |
| --------------------- | ------------------------ |
| .NET                  | 10                       |
| ASP.NET Core          | 10                       |
| C#                    | Latest supported version |
| Entity Framework Core | 10                       |
| Database              | SQL Server               |
| API Documentation     | Swagger / OpenAPI        |
| Querying              | LINQ                     |
| Data Access           | Entity Framework Core    |
| Version Control       | Git / GitHub             |

---

## 📁 Project Structure

```text
StudentManagementSystem/
│
├── Controllers/
│   └── StudentsController.cs
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── Models/
│   └── Student.cs
│
├── DTOs/
│   ├── StudentCreateDto.cs
│   ├── StudentUpdateDto.cs
│   └── StudentResponseDto.cs
│
├── Services/
│   └── StudentService.cs
│
├── Migrations/
│
├── Properties/
│   └── launchSettings.json
│
├── appsettings.json
├── Program.cs
├── StudentManagementSystem.csproj
└── README.md
```

> The folder structure may vary depending on the implementation of the project.

---

# 🗄️ Database

The application uses **Microsoft SQL Server** with **Entity Framework Core**.

### Student Table

Example student fields:

| Field       | Type     | Description          |
| ----------- | -------- | -------------------- |
| Id          | int      | Primary Key          |
| Name        | string   | Student name         |
| Email       | string   | Student email        |
| Phone       | string   | Contact number       |
| Course      | string   | Student course       |
| Age         | int      | Student age          |
| CreatedDate | DateTime | Record creation date |

---

# 🔌 API Endpoints

## 1. Get All Students

```http
GET /api/students
```

Returns a paginated list of students.

### Example

```http
GET /api/students?pageNumber=1&pageSize=10
```

---

## 2. Search Students

```http
GET /api/students?search=John&pageNumber=1&pageSize=10
```

The search functionality allows students to be searched using supported fields such as name, email, course, etc.

### Example

```text
Search: John
Page Number: 1
Page Size: 10
```

---

## 3. Get Student By ID

```http
GET /api/students/{id}
```

### Example

```http
GET /api/students/1
```

---

## 4. Create Student

```http
POST /api/students
```

### Example Request

```json
{
  "name": "John Smith",
  "email": "john.smith@example.com",
  "phone": "9876543210",
  "course": "Computer Science",
  "age": 21
}
```

---

## 5. Update Student

```http
PUT /api/students/{id}
```

### Example

```http
PUT /api/students/1
```

### Request Body

```json
{
  "name": "John Updated",
  "email": "john.updated@example.com",
  "phone": "9876543210",
  "course": "Information Technology",
  "age": 22
}
```

---

## 6. Delete Student

```http
DELETE /api/students/{id}
```

### Example

```http
DELETE /api/students/1
```

---

# 📄 CSV Export

The API provides an endpoint to download student records as a CSV file.

```http
GET /api/students/export
```

The generated CSV file can be opened using:

* Microsoft Excel
* Google Sheets
* LibreOffice Calc
* Any CSV-compatible application

### Example CSV

```csv
Id,Name,Email,Phone,Course,Age
1,John Smith,john@example.com,9876543210,Computer Science,21
2,David Brown,david@example.com,9876543211,Information Technology,22
```

---

# 📑 Pagination

The API implements **server-side pagination** to avoid loading the complete student table into memory.

### Request

```http
GET /api/students?pageNumber=2&pageSize=10
```

### Parameters

| Parameter    | Description                | Example |
| ------------ | -------------------------- | ------- |
| `pageNumber` | Page number                | `1`     |
| `pageSize`   | Number of records per page | `10`    |
| `search`     | Search keyword             | `John`  |

### Example

```http
GET /api/students?search=John&pageNumber=1&pageSize=10
```

A typical paginated response can contain:

```json
{
  "data": [
    {
      "id": 1,
      "name": "John Smith",
      "email": "john@example.com"
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalRecords": 25,
  "totalPages": 3
}
```

---

# 🔎 Search + Pagination

Search and pagination can be combined in a single API request.

```http
GET /api/students?search=Computer&pageNumber=1&pageSize=10
```

The expected processing flow is:

```text
Client
   ↓
Controller
   ↓
Search / Filtering
   ↓
Pagination
   ↓
Entity Framework Core
   ↓
SQL Server
   ↓
API Response
```

This approach allows the database to return only the records required for the requested page.

---

# 🧩 Entity Framework Core

The project uses **Entity Framework Core** for database operations.

Typical database operations include:

```csharp
_dbContext.Students.AddAsync(student);
_dbContext.Students.ToListAsync();
_dbContext.Students.FindAsync(id);
_dbContext.SaveChangesAsync();
```

LINQ is used for filtering and pagination.

Example:

```csharp
var students = await _dbContext.Students
    .Where(x => x.Name.Contains(search))
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

---

# 🔄 CRUD Flow

The application follows the standard CRUD workflow:

```text
Create
   ↓
POST /api/students
   ↓
SQL Server

Read
   ↓
GET /api/students
   ↓
Search + Pagination
   ↓
SQL Server

Update
   ↓
PUT /api/students/{id}
   ↓
SQL Server

Delete
   ↓
DELETE /api/students/{id}
   ↓
SQL Server
```

---

# ⚙️ Prerequisites

Before running the project, install:

* [.NET 10 SDK](https://dotnet.microsoft.com/)
* SQL Server
* SQL Server Management Studio (SSMS) or another SQL client
* Visual Studio 2022/2026 or Visual Studio Code
* Git

---

# 📥 Clone Repository

Clone the repository:

```bash
git clone https://github.com/Jaimindeveloper/StudentManagementSystem-DotNet10.git
```

Navigate to the project directory:

```bash
cd StudentManagementSystem-DotNet10
```

---

# 📦 Restore Dependencies

Run:

```bash
dotnet restore
```

---

# 🗄️ Configure Database

Update the SQL Server connection string in your configuration.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=StudentManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

For SQL Server authentication:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=StudentManagementDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  }
}
```

> Do not commit real database passwords, API keys, or other secrets to GitHub.

---

# 🏗️ Entity Framework Core Migration

If migrations are already included in the repository, run:

```bash
dotnet ef database update
```

If Entity Framework CLI is not installed:

```bash
dotnet tool install --global dotnet-ef
```

To create a new migration:

```bash
dotnet ef migrations add InitialCreate
```

Then update the database:

```bash
dotnet ef database update
```

---

# ▶️ Run the Application

Run the API using:

```bash
dotnet run
```

Or:

```bash
dotnet watch run
```

The application will start on the configured HTTP/HTTPS ports.

---

# 📚 Swagger API Documentation

After starting the application, open Swagger in your browser.

```text
https://localhost:<port>/swagger
```

Swagger allows you to:

* View all API endpoints
* Test CRUD operations
* Test search
* Test pagination
* Test CSV export
* View request and response models

---

# 🧪 API Testing

The APIs can be tested using:

* Swagger
* Postman
* REST Client
* Visual Studio
* curl

Example using curl:

```bash
curl "https://localhost:<port>/api/students?pageNumber=1&pageSize=10"
```

---

# 🧠 Key Concepts Demonstrated

This project focuses on practical ASP.NET Core development concepts.

### ASP.NET Core

* Web API
* Controllers
* Routing
* Dependency Injection
* Configuration
* Middleware
* HTTP status codes

### C#

* Classes
* Interfaces
* Async/Await
* LINQ
* Generics
* Exception handling

### Entity Framework Core

* DbContext
* DbSet
* Migrations
* LINQ queries
* CRUD operations
* SQL Server integration

### API Development

* RESTful API design
* DTOs
* Validation
* Search
* Pagination
* File download
* CSV generation

---

# 📈 Future Enhancements

The project can be extended with additional enterprise-level features:

* 🔐 JWT Authentication
* 👤 Role-Based Authorization
* 🛡️ Global Exception Handling Middleware
* ✅ FluentValidation
* 📋 Repository Pattern
* 📦 Unit of Work Pattern
* 📝 Structured Logging
* ⚡ Redis Caching
* 📊 Excel Import
* 📊 Excel Export
* 🧪 Unit Testing
* 🧪 Integration Testing
* 🐳 Docker
* ☁️ Azure Deployment
* 🔄 CI/CD Pipeline
* 📖 API Versioning
* 🔍 Advanced Filtering and Sorting

---

# 🎯 Learning Objective

The main objective of this project is to understand how to build a practical **ASP.NET Core 10 Web API** from the ground up.

The project covers the complete flow:

```text
Client
   ↓
HTTP Request
   ↓
Controller
   ↓
Service / Business Logic
   ↓
Entity Framework Core
   ↓
SQL Server
   ↓
Database Result
   ↓
DTO
   ↓
HTTP Response
   ↓
Client
```

---

# 📌 Project Highlights

This project demonstrates:

```text
ASP.NET Core 10
       +
Entity Framework Core
       +
SQL Server
       +
CRUD
       +
Search
       +
Pagination
       +
CSV Export
       +
Swagger
```

It can be used as a reference project for learning **modern .NET Web API development** and as a portfolio project demonstrating practical backend development skills.

---

# 👨‍💻 Author

**Jaimin Suthar**

### GitHub

https://github.com/Jaimindeveloper

### LinkedIn

https://www.linkedin.com/in/jaimin-suthar12/

---

# ⭐ Support

If you find this project useful for learning ASP.NET Core, consider giving the repository a ⭐ on GitHub.

---

## 📄 License

This project is intended for learning and portfolio purposes.
