# Simple API with DynamoDB

This API demonstrates using a hybrid database approach, with **PostgreSQL** for write operations and **DynamoDB** for read operations, providing a high-level introduction to integrating DynamoDB in a .NET project.

## Purpose

This API provides an example of combining SQL and NoSQL databases to handle different types of operations efficiently, offering insight into DynamoDB’s basics alongside PostgreSQL.

## Technologies

- **ASP.NET Core**: Framework for building the API.
- **PostgreSQL**: Used for reliable write operations.
- **AWS DynamoDB**: Used for fast, scalable read operations.

## Endpoints

### `POST /api/v1/customers`
- Adds a new customer to PostgreSQL.

### `GET /api/v1/customers`
- Retrieves all customers from DynamoDB.

### `GET /api/v1/customers/{id}`
- Retrieves a customer from DynamoDB by their unique ID.

### `GET /api/v1/customers/email`
- Retrieves a customer from DynamoDB by their email.

### `DELETE /api/v1/customers/{id}`
- Deletes a customer from PostgreSQL by their ID.

---

This project serves as a learning tool for developers interested in integrating DynamoDB with traditional relational databases.
