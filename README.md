# 🖥️ Todo API – Finals_Q1 (Backend)

This is the backend API for the Todo application built using **ASP.NET Core Web API**.
It provides CRUD operations, blockchain-style validation, and proof-of-work security.

---

# 🚀 Project Overview

The backend handles:

* Todo data storage (in-memory)
* CRUD API endpoints
* Hash chain generation for data integrity
* Proof-of-work validation
* Chain verification

---

# 🧩 Architecture Summary

The backend follows a **layered architecture**:

* **Controller Layer** → Handles HTTP requests
* **Model Layer** → Defines Todo structure
* **Helper Layer** → Hashing logic (SHA-256)
* **In-Memory Storage** → `List<Todo>`

### Key Patterns:

* RESTful API design
* Separation of concerns
* Deterministic hash chaining

---

# ⚙️ Setup Instructions

```bash
cd TodoApi
dotnet restore
dotnet run
```

API runs at:

```
http://localhost:5154
```

Swagger:

```
http://localhost:5154/swagger
```

---

# 🔗 API Endpoints

| Method | Endpoint          | Description       |
| ------ | ----------------- | ----------------- |
| GET    | /api/todos        | Get all todos     |
| POST   | /api/todos        | Create todo       |
| PUT    | /api/todos/{id}   | Update todo       |
| DELETE | /api/todos/{id}   | Delete todo       |
| GET    | /api/todos/verify | Verify hash chain |

---

# 🔐 Advanced Features

## Blockchain-Style Hashing

* Each todo contains:

  * `Hash`
  * `PreviousHash`
* Each item links to the previous item
* Any modification breaks the chain

---

## Chain Verification

```
GET /api/todos/verify
```

Returns:

* `200 OK` → Chain Valid
* `409 Conflict` → Chain Tampered

---

## ⛏️ Proof-of-Work

Before accepting POST/PUT:

```
SHA256(title | nonce) must start with "00"
```

* Valid → accepted
* Invalid → rejected (`400 Bad Request`)

---

# 📌 Summary

This backend demonstrates:

* RESTful API development
* Hash-based data integrity
* Proof-of-work validation
* Clean architecture design

---

# 👨‍💻 Author

**FRANCO, RYAN CHRISTIAN C.**
