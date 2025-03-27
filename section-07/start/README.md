# Order Management API

This is a demo API for managing orders. The API supports two types of users: Admin and Regular User, with different levels of access to the endpoints.

## Getting Started

1. Start the database using Docker Compose:
   ```bash
   docker-compose up -d
   ```
   This will start a PostgreSQL database on port 5432.

2. Start the application
3. Navigate to the Swagger UI (typically at `https://localhost:5001/swagger`)

## Seeded Data

The application comes with pre-seeded data for testing purposes:

### Customer
- ID: 1
- Name: "Gui"
- Email: "gui@guiferreira.me"
- Phone Number: "1234567890"

### Product
- ID: 1
- Name: "Computer"
- Price: 1000
- Stock Quantity: 10
- Description: "What a great computer!"

You can use these IDs when creating orders in the API.

## Authentication

The API uses JWT (JSON Web Token) for authentication. You need to authenticate before accessing protected endpoints.

### Available Test Users

1. **Admin User**
   - Email: `admin@example.com`
   - Password: `admin123`
   - Role: `Admin`
   - Access: Full access to all endpoints

2. **Regular User**
   - Email: `user@example.com`
   - Password: `user123`
   - Role: `User`
   - Access: Limited access to endpoints

### How to Authenticate in Swagger

1. Find the `POST /api/Auth/login` endpoint
2. Click "Try it out"
3. Enter the credentials in the request body:
   ```json
   {
     "email": "admin@example.com",
     "password": "admin123"
   }
   ```
   or
   ```json
   {
     "email": "user@example.com",
     "password": "user123"
   }
   ```
4. Click "Execute"
5. Copy the token from the response
6. Click the "Authorize" button at the top of the Swagger UI (looks like a lock icon)
7. Enter `Bearer ` followed by your token
8. Click "Authorize"
9. Close the dialog

## Available Endpoints

### Orders

- `GET /api/Orders` - Get all orders

- `GET /api/Orders/{id}` - Get a specific order

- `POST /api/Orders` - Create a new order

- `PUT /api/Orders/{id}/cancel` - Cancel an order
  - Access: Admin only

## Important Notes

1. This is a demo application with simplified authentication
2. In a production environment:
   - Use proper user management
   - Store credentials securely
   - Use HTTPS
   - Implement proper security measures
3. The JWT token expires after 1 hour
4. All timestamps are in UTC
5. The database is automatically created and seeded on first run

## Testing Different Roles

To test different roles:
1. Log out (clear the authorization in Swagger)
2. Login with different credentials
3. Try accessing different endpoints to see the role-based restrictions

