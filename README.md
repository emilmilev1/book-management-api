# Book Management API

A simple API for managing books in a library system.

---

## Technologies

-   **.NET 8**
-   **SQL Server**
-   **JWT Authentication**
-   **Swagger** for API documentation and testing

---

## Setup & Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/emilmilev1/book-management-api.git
    cd book-management-api
    ```

2. Configure the database connection in `appsettings.json`.

3. Run the application:
    ```bash
    dotnet run
    ```

---

## API Endpoints

### Authentication

**POST /api/auth/login**

This is a hardcoded user.

-   **Request**:

    ```json
    {
        "username": "admin",
        "password": "password"
    }
    ```

-   **Response**:
    ```json
    {
        "token": "your-jwt-token"
    }
    ```

---

### Books

-   **GET /api/books/titles** - Get book titles (pagination).
-   **GET /api/books/{id}** - Get book details by ID.
-   **POST /api/books/book** - Add a new book.
-   **POST /api/books** - Add some books.
-   **PUT /api/books/{id}** - Update book details.
-   **DELETE /api/books/{id}** - Soft delete a book.
-   **DELETE /api/books** - Soft delete some books.

---

## Using Swagger UI

1. Run the API and navigate to `https://localhost:5001/swagger` in your browser.
2. Sign in the user and copy the token.
3. Click **Authorize** and paste the JWT token (in format `Bearer <your-token>`).
4. Use the "Try it out" buttons to interact with the API endpoints.

---

## Testing with Postman

1. **Login**: Send a `POST` request to `/api/auth/login` with the username and password to receive a JWT token.
2. **Add Token**: Use the received token as a Bearer token in the **Authorization** header for secured endpoints.

---

Have fun with your books!
