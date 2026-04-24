Car Sales Management System

A full-stack Car Sales Management Website built with ASP.NET Core MVC, C#, Entity Framework Core, SQL Server, Bootstrap, HTML, CSS, and JavaScript.

This system allows customers to browse, search, and purchase vehicles online, while admins manage inventory, sales, bookings, users, and customer enquiries through a separate dashboard.

Features

*Customer Features
- Register and log in
- Browse available cars
- Search and filter cars by brand, price, fuel type, transmission, and condition
- View detailed car information
- Save cars to favorites
- Book test drives
- Submit purchase requests
- Use a demo Stripe-style test payment option
- View purchase history
- Contact the dealership

*Admin Features
- Admin dashboard
- Add, edit, and delete car listings
- Manage bookings
- Manage purchases
- Update car availability
- Manage users
- View customer messages
- Receive separated admin-only vehicle management pages

*Business Rules
- Only available vehicles can be booked or purchased
- Once a customer submits a purchase request, the car is marked as reserved
- Completed purchases mark the car as sold
- Rejected purchases return the car to available stock
- Admin users cannot perform customer purchase actions
- Purchase confirmation/update emails are supported through SMTP configuration

*Tech Stack
- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server / SSMS
- ASP.NET Core Identity
- Bootstrap 5
- HTML / CSS / JavaScript

* Database
The project uses SQL Server with Entity Framework Core migrations.

Current database connection:
- `CarSalesManagementDB`

Main tables include:
- AspNetUsers
- Cars
- Bookings
- Purchases
- Payments
- Favorites
- ContactMessages

* Test Accounts

* Admin
- Email: `admin@motormart.local`
- Password: `Admin@12345`

* Customer
- Email: `customer@motormart.local`
- Password: `Customer@12345`

* How to Run the Project

1. Clone or download the project
2. Open the project in Visual Studio
3. Make sure SQL Server Express is installed
4. Update the connection string in `appsettings.json` if needed
5. Run the migrations if required
6. Start the application

### Connection String
```json
"DefaultConnection": "Server=BO\\SQLEXPRESS;Database=CarSalesManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
