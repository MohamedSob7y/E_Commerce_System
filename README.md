# E-Commerce RESTful API — .NET 8

A backend **E-Commerce RESTful API** built with **ASP.NET Core .NET 8**, following a layered architecture and applying several common backend design patterns such as **Repository**, **Unit of Work**, **Specification**, **Dependency Injection**, and **Middleware**.

The application provides product management, filtering, searching, server-side pagination, Redis-based basket storage and caching, centralized exception handling, automatic database migration and seeding, DTO mapping, and Swagger/OpenAPI documentation.

---

## 📌 Project Overview

This project represents the backend of an E-Commerce application.

It exposes RESTful endpoints that can be consumed by any frontend application such as:

* React
* Angular
* Vue
* Mobile applications
* Desktop applications
* Other external services

The API is responsible for handling business logic, product data, database communication, caching, basket management, pagination, filtering, searching, mapping entities to DTOs, and returning structured HTTP responses to clients.

---

# 🛠️ Technologies Used

The project is built using the following technologies:

* **C#**
* **.NET 8**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **SQL Server**
* **Redis**
* **StackExchange.Redis**
* **AutoMapper**
* **Swagger / OpenAPI**
* **Dependency Injection**
* **LINQ**
* **RESTful API principles**

---

# 🏗️ Architecture

The project follows a **Layered Architecture** to separate responsibilities and keep the application maintainable, testable, and easier to extend.

The solution is divided into the following layers:

```text
Presentation
     ↓
Application / Services
     ↓
Domain
     ↓
Persistence

Shared
```

Each layer has a specific responsibility.

---

## 1. Presentation Layer

The Presentation layer contains the ASP.NET Core Web API controllers.

Example:

```text
ProductController
```

Controllers are responsible for:

* Receiving HTTP requests
* Reading route/query parameters
* Calling the appropriate service
* Returning HTTP responses
* Applying API-level attributes such as caching

Controllers intentionally contain minimal business logic.

For example:

```text
GET /api/products
```

A request reaches the `ProductController`, which delegates the actual work to the `IProductService`.

This keeps controllers thin and separates HTTP concerns from application logic.

---

## 2. Application / Services Layer

The Services layer contains the application's business logic and coordinates operations between the Presentation and Persistence layers.

Examples:

```text
IProductService
ProductService

IBasketService
BasketService

ICacheService
CacheService
```

The project separates service interfaces from their implementations.

For example:

```text
IProductService
      ↓
ProductService
```

This provides several benefits:

* Loose coupling
* Better testability
* Easier replacement of implementations
* Cleaner Dependency Injection
* Separation of concerns

The Product Service is responsible for operations such as:

* Retrieving products
* Applying filtering criteria
* Handling pagination
* Coordinating specifications
* Mapping entities to DTOs
* Returning structured results to controllers

---

# 🗄️ Persistence Layer

The Persistence layer handles communication with external data stores.

The project mainly uses:

```text
SQL Server
Redis
```

It contains components such as:

```text
StoreDBContext
GenericRepository
BasketRepository
UnitOfWork
SpecificationEvaluator
```

This layer hides database implementation details from the business layer.

---

# 🧩 Domain Layer

The Domain layer contains the core entities and contracts of the application.

Examples of domain entities may include:

```text
Product
Brand
Category
Basket
```

The Domain layer represents the core business model and should remain independent from infrastructure details whenever possible.

For example, a `Product` entity should not need to know whether it is stored in SQL Server, PostgreSQL, or another database.

This separation reduces coupling between business models and infrastructure.

---

# 📦 Shared Layer

The Shared layer contains reusable models and utilities that can be used by multiple layers.

Examples include:

```text
DTOs
Pagination models
Specifications
Mapping profiles
Mapping resolvers
Query parameters
Common utilities
```

Examples:

```text
ProductDTO
ProductQueryParam
PaginatedResult<T>
ProductPictureUrlResolver
```

---

# 🛍️ Product Management

The API provides CRUD operations for products.

CRUD stands for:

```text
Create
Read
Update
Delete
```

Typical REST endpoints include:

```http
GET    /api/products
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
```

These endpoints allow clients to manage products through standard REST conventions.

---

# 🔎 Product Filtering

The API supports dynamic product filtering.

Products can be filtered using query parameters such as:

```text
Brand
Category
Search keyword
Page index
Page size
```

Example:

```http
GET /api/products?brandId=2&categoryId=1
```

Filtering logic is encapsulated using the **Specification Pattern** rather than placing complex LINQ expressions directly inside controllers.

---

# 🔍 Product Search

Clients can search for products using query parameters.

Example:

```http
GET /api/products?search=laptop
```

The search value is included inside the product specification and translated by Entity Framework Core into the appropriate SQL query.

---

# 📄 Server-Side Pagination

The project implements server-side pagination to avoid returning very large product collections in a single request.

Example:

```http
GET /api/products?pageIndex=1&pageSize=10
```

Instead of returning thousands of products, the API returns only the requested page.

A typical response contains information similar to:

```json
{
  "pageIndex": 1,
  "pageSize": 10,
  "totalCount": 150,
  "data": []
}
```

Pagination is implemented using:

```text
PaginatedResult<T>
```

together with the Specification Pattern.

This improves:

* API performance
* Database performance
* Network usage
* Frontend user experience

---

# 🧠 Specification Pattern

The project uses the **Specification Pattern** to encapsulate query logic.

Without specifications, repositories or services can quickly become filled with complex conditions such as:

```text
Filter by brand
Filter by category
Search by name
Sorting
Pagination
Includes
```

Instead, query requirements are represented inside specification objects.

The general flow is:

```text
Product Query Parameters
        ↓
Specification
        ↓
SpecificationEvaluator
        ↓
IQueryable<Product>
        ↓
Entity Framework Core
        ↓
SQL Server
```

The project uses:

```text
ISpecification
SpecificationEvaluator
```

Repository methods such as:

```text
GetAllAsync
GetByIdAsync
CountAsync
```

can accept specifications.

### Why Specification Pattern?

It provides:

* Reusable query logic
* Cleaner repositories
* Cleaner services
* Easier pagination
* Easier filtering
* Better separation of concerns
* Less duplicated LINQ code

---

# 🗃️ Repository Pattern

The project applies the **Repository Pattern** to encapsulate database access.

Instead of services communicating directly with `DbContext`, they communicate through repositories.

```text
Service
   ↓
Repository
   ↓
Entity Framework Core
   ↓
SQL Server
```

This separates business logic from database access logic.

---

# 🔁 Generic Repository

A generic repository is used for common database operations.

Instead of creating the same methods repeatedly for every entity:

```text
ProductRepository
CategoryRepository
BrandRepository
```

the project provides reusable functionality through:

```text
GenericRepository<T>
```

Common operations can include:

```text
GetAllAsync
GetByIdAsync
Add
Update
Delete
CountAsync
```

This reduces duplicated data-access code.

---

# 🔄 Unit of Work Pattern

The project implements the **Unit of Work Pattern**.

Components:

```text
IUnitOfWork
UnitOfWork
```

The Unit of Work coordinates repositories and database changes through a single point.

Conceptually:

```text
Product Repository
       │
Category Repository
       │
Brand Repository
       ↓
   Unit Of Work
       ↓
   SaveChanges
       ↓
   SQL Server
```

The main goal is to coordinate related database operations and provide a common commit operation.

This becomes particularly useful when one business operation modifies data through multiple repositories.

---

# 🗄️ SQL Server

**SQL Server** is used as the main relational database.

Persistent application data such as products, brands, and categories is stored in SQL Server.

Database access is handled through **Entity Framework Core**.

---

# 🔗 Entity Framework Core

The project uses **Entity Framework Core** as its ORM.

ORM stands for:

```text
Object-Relational Mapper
```

Entity Framework Core maps C# entities to relational database tables.

For example:

```csharp
Product
```

can be mapped to:

```text
Products table
```

Instead of manually writing most SQL queries, the application can use LINQ:

```csharp
await context.Products.ToListAsync();
```

Entity Framework Core translates the LINQ expression into SQL and executes it against SQL Server.

---

# 🧱 Database Migrations

Entity Framework Core migrations are used to manage changes to the database schema.

For example, if a new property is added to a Product entity, a migration can represent the required database modification.

The application automatically applies pending migrations during startup using functionality similar to:

```text
MigrateDatabaseAsync
```

This ensures the application's expected schema and database schema remain synchronized during development.

> For larger production systems, database migrations are commonly executed as a separate deployment step rather than automatically during application startup.

---

# 🌱 Data Seeding

The project supports automatic initial data seeding.

When the application starts, predefined data can be inserted into the database if necessary.

This can include data such as:

```text
Products
Brands
Categories
```

The seeding process is handled through functionality such as:

```text
SeedDataAsync
```

This makes development and testing easier because the project can start with useful sample data.

---

# ⚡ Redis

The project integrates **Redis** using:

```text
StackExchange.Redis
```

Redis is a high-performance in-memory data store.

Because Redis primarily works with memory rather than traditional disk-based database access, it is suitable for frequently accessed or temporary data.

The project uses Redis mainly for:

* Basket storage
* Caching

---

# 🛒 Redis-Based Basket

Shopping basket data is stored in Redis.

Basket data changes frequently because users may:

* Add items
* Remove items
* Update quantities
* Clear their basket

Redis is suitable for this use case because basket operations require fast reads and writes.

Conceptually:

```text
Client
   ↓
Basket Controller / Service
   ↓
Basket Repository
   ↓
Redis
```

A basket can be associated with a Redis key such as:

```text
basket:user-id
```

and stored as serialized data.

---

# 🔌 StackExchange.Redis

The .NET application communicates with Redis using the:

```text
StackExchange.Redis
```

library.

The Redis connection is represented by:

```text
IConnectionMultiplexer
```

and is registered as a Singleton inside the Dependency Injection container.

Example concept:

```csharp
AddSingleton<IConnectionMultiplexer>()
```

A single Redis connection multiplexer can therefore be reused throughout the application lifetime rather than opening a new connection for every HTTP request.

---

# 🚀 Redis Caching

The project includes Redis-based caching to avoid repeating expensive operations unnecessarily.

Conceptually:

```text
Request
   ↓
Check Cache
   ↓
Is Cached?
  /       \
Yes       No
 |         |
Return     Execute Service
Cached          ↓
Data        Database
              ↓
          Cache Result
              ↓
           Response
```

Caching can reduce:

* Database queries
* API response time
* Server workload

and improve application performance.

---

# 🏷️ Attribute-Based Caching

Caching is implemented through an attribute-based mechanism.

For example:

```csharp
[RedisCache]
```

can be applied to an API endpoint.

The attribute intercepts the request and checks whether an appropriate cached response already exists.

If the response exists:

```text
Redis → Cached Response → Client
```

If not:

```text
Controller
   ↓
Service
   ↓
Database
   ↓
Response
   ↓
Redis Cache
```

This keeps caching logic outside the main controller and service business logic.

Caching is a **cross-cutting concern**, so separating it from core business operations keeps the application cleaner.

This approach is similar to **Aspect-Oriented Programming (AOP)** because reusable behavior is applied around endpoint execution.

---

# 💉 Dependency Injection

ASP.NET Core's built-in Dependency Injection container is used throughout the project.

Instead of manually creating dependencies:

```csharp
var productService = new ProductService(...);
```

services are registered centrally inside the application composition root.

Example:

```csharp
builder.Services.AddScoped<IProductService, ProductService>();
```

Controllers can then request interfaces through constructor injection.

Example:

```csharp
public ProductController(IProductService productService)
{
    _productService = productService;
}
```

ASP.NET Core automatically resolves and injects the required implementation.

Benefits include:

* Loose coupling
* Easier unit testing
* Better maintainability
* Centralized dependency configuration
* Easier replacement of implementations

---

# ♻️ Dependency Injection Lifetimes

The project uses different DI lifetimes depending on the responsibility of each component.

## Scoped

Scoped services create one instance per HTTP request.

Used for components such as:

```text
Services
Repositories
UnitOfWork
DbContext
```

Example:

```csharp
AddScoped<IProductService, ProductService>();
```

---

## Singleton

Singleton services use one shared instance during the entire application lifetime.

Redis `IConnectionMultiplexer` is registered as Singleton because Redis connections are designed to be reused.

Example:

```csharp
AddSingleton<IConnectionMultiplexer>();
```

---

## Transient

Transient services create a new instance every time they are requested.

This can be useful for lightweight and stateless components such as mapping resolvers.

Example:

```csharp
AddTransient<ProductPictureUrlResolver>();
```

---

# 📤 DTOs

The project uses **Data Transfer Objects (DTOs)** instead of returning domain/database entities directly to API clients.

Example:

```text
Product Entity
      ↓
ProductDTO
      ↓
JSON Response
```

DTOs allow the application to control exactly which information is returned to clients.

Benefits include:

* Hiding internal entity implementation
* Protecting API contracts
* Reducing unnecessary response properties
* Preventing tight coupling between API consumers and database models
* Providing client-specific response shapes

---

# 🔄 AutoMapper

**AutoMapper** is used to convert entities into DTOs.

Instead of manually writing mapping code for every property:

```text
Product
   ↓
AutoMapper
   ↓
ProductDTO
```

mapping profiles define the transformation rules.

This reduces repetitive mapping code and keeps transformations centralized.

---

# 🖼️ ProductPictureUrlResolver

The project contains a custom AutoMapper resolver:

```text
ProductPictureUrlResolver
```

Its purpose is to generate the final URL of a product image.

For example, the database may contain a relative path:

```text
images/products/product1.png
```

but the API client needs a complete URL such as:

```text
https://localhost:5001/images/products/product1.png
```

The resolver transforms the stored image path into the URL expected by API consumers.

Conceptually:

```text
Database Image Path
        ↓
ProductPictureUrlResolver
        ↓
Complete Image URL
        ↓
ProductDTO
```

---

# 🧯 Centralized Exception Handling

The project implements centralized exception handling using custom middleware.

Component:

```text
ExceptionHandlerMiddleware
```

Instead of repeating `try/catch` blocks inside every controller, unexpected exceptions can be handled from a single location.

HTTP pipeline example:

```text
HTTP Request
     ↓
ExceptionHandlerMiddleware
     ↓
Controller
     ↓
Service
     ↓
Repository
```

If an exception occurs anywhere further down the pipeline:

```text
Exception
   ↓
ExceptionHandlerMiddleware
   ↓
Consistent Error Response
   ↓
Client
```

A standardized response may look similar to:

```json
{
  "statusCode": 500,
  "message": "Internal Server Error"
}
```

Benefits:

* Centralized error handling
* Cleaner controllers
* Consistent API responses
* Easier logging and maintenance

---

# 📖 Swagger / OpenAPI

The API includes Swagger/OpenAPI documentation.

Swagger provides an interactive interface where developers can:

* View available endpoints
* Inspect request parameters
* View request/response models
* Send API requests
* Test endpoints without a separate frontend

Swagger is enabled in the **Development environment**.

Conceptually:

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

This prevents development API documentation from being automatically exposed in environments where it is not required.

---

# 🔒 HTTPS

The API uses HTTPS redirection.

Requests sent using:

```text
HTTP
```

can be redirected to:

```text
HTTPS
```

to provide encrypted communication between clients and the API.

---

# 🖼️ Static File Serving

ASP.NET Core static file middleware is enabled.

This allows files such as product images to be served from the application.

For example:

```text
wwwroot/images/products/
```

can expose images using URLs such as:

```text
https://localhost:5001/images/products/product.png
```

This integrates with `ProductPictureUrlResolver` when generating Product DTO responses.

---

# 🔄 Complete Request Flow

A product request demonstrates how the different project layers work together.

For example:

```http
GET /api/products?brandId=2&pageIndex=1&pageSize=10
```

The request can follow this flow:

```text
Client
  ↓
ASP.NET Core Middleware Pipeline
  ↓
Caching Layer / Attribute
  ↓
ProductController
  ↓
IProductService
  ↓
ProductService
  ↓
Specification
  ↓
UnitOfWork
  ↓
GenericRepository
  ↓
SpecificationEvaluator
  ↓
Entity Framework Core
  ↓
SQL Server
  ↓
Product Entities
  ↓
AutoMapper
  ↓
ProductDTO
  ↓
PaginatedResult<ProductDTO>
  ↓
HTTP JSON Response
  ↓
Client
```

This flow demonstrates the main goal of the architecture:

> Each component has a clear responsibility instead of placing database access, business logic, mapping, caching, and HTTP handling inside the same class.

---

# 🎯 Design Patterns & Techniques

The project applies several backend design patterns and architectural techniques.

## Repository Pattern

Used to encapsulate Entity Framework Core data-access operations.

**Purpose:**

* Separate business logic from database access
* Reduce coupling with EF Core
* Centralize data access behavior

---

## Generic Repository

Provides reusable CRUD/data-access functionality for multiple entities.

**Purpose:**

* Reduce duplicate repository code
* Reuse common database operations

---

## Unit of Work

Coordinates repository operations and database commits.

**Purpose:**

* Provide a central commit operation
* Coordinate multiple repositories
* Help manage logically related database changes

---

## Specification Pattern

Encapsulates filtering, searching, pagination, sorting, and query criteria.

**Purpose:**

* Keep repositories clean
* Reuse query logic
* Support complex dynamic queries
* Avoid duplicated LINQ expressions

---

## Dependency Injection

Interfaces and implementations are registered through ASP.NET Core's built-in DI container.

**Purpose:**

* Loose coupling
* Testability
* Centralized dependency management

---

## Middleware Pattern

Custom middleware handles cross-cutting behavior such as exception handling.

**Purpose:**

* Centralized behavior
* Cleaner controllers and services
* Consistent API behavior

---

## Attribute-Based Caching

Redis caching can be applied to endpoints using reusable attributes.

**Purpose:**

* Keep caching outside business logic
* Reuse cache behavior
* Reduce database calls
* Improve response time

---

## DTO Pattern

DTOs define the API response/request contracts independently from domain entities.

**Purpose:**

* Protect domain entities
* Control API output
* Reduce coupling with persistence models

---

## Mapping / Resolver Pattern

AutoMapper and custom resolvers transform domain models into client-friendly DTOs.

**Purpose:**

* Centralize mapping logic
* Reduce repetitive transformation code
* Handle derived values such as image URLs

---

# 📁 Simplified Project Structure

A simplified representation of the solution structure:

```text
ECommerce/
│
├── Presentation/
│   ├── Controllers/
│   │   └── ProductController.cs
│   └── Middleware/
│       └── ExceptionHandlerMiddleware.cs
│
├── Services/
│   ├── Interfaces/
│   │   ├── IProductService.cs
│   │   └── IBasketService.cs
│   └── Implementations/
│       ├── ProductService.cs
│       └── BasketService.cs
│
├── Persistence/
│   ├── Data/
│   │   └── StoreDBContext.cs
│   ├── Repositories/
│   │   ├── GenericRepository.cs
│   │   └── BasketRepository.cs
│   ├── UnitOfWork/
│   └── Migrations/
│
├── Domain/
│   ├── Entities/
│   └── Contracts/
│
├── Shared/
│   ├── DTOs/
│   ├── Mapping/
│   ├── Specifications/
│   └── Pagination/
│
└── Program.cs
```

> The exact folder names may differ depending on the final repository structure.

---

# ⚙️ Application Startup

`Program.cs` acts as the application's **Composition Root**.

It is responsible for configuring:

```text
Controllers
Dependency Injection
Entity Framework Core
SQL Server
Redis
AutoMapper
Swagger
Middleware
HTTPS
Static Files
Database migrations
Database seeding
```

The general startup flow is:

```text
Application Starts
       ↓
Register Services
       ↓
Configure SQL Server
       ↓
Configure Redis
       ↓
Configure AutoMapper
       ↓
Configure Swagger
       ↓
Build Application
       ↓
Apply Migrations
       ↓
Seed Initial Data
       ↓
Configure Middleware Pipeline
       ↓
Map Controllers
       ↓
Application Ready
```

---

# 🚦 Getting Started

## Prerequisites

Make sure the following are installed:

* .NET 8 SDK
* SQL Server
* Redis
* Git
* Visual Studio / Visual Studio Code / Rider

---

## Clone the Repository

```bash
git clone <your-repository-url>
```

Navigate to the project directory:

```bash
cd <project-directory>
```

---

## Configure Database Connection

Update the SQL Server connection string inside your application configuration file.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Use the connection string format appropriate for your environment.

---

## Configure Redis

Make sure Redis is running and configure the Redis connection used by the application.

Example:

```text
localhost:6379
```

---

## Restore Dependencies

```bash
dotnet restore
```

---

## Run the Application

```bash
dotnet run
```

The application will automatically perform the configured database migration and seeding operations during startup.

---

# 🧪 Testing the API

When the project is running in Development mode, Swagger can be used to test the API.

Open the Swagger endpoint shown in the console after starting the application.

From Swagger you can test operations such as:

```text
GET products
GET product by ID
Create product
Update product
Delete product
Filter products
Search products
Paginate products
Basket operations
```

---

# 💡 Key Engineering Decisions

### Why Layered Architecture?

To separate HTTP concerns, business logic, domain models, and database access.

---

### Why Repository Pattern?

To keep Entity Framework Core data-access logic outside the service layer.

---

### Why Specification Pattern?

Product queries can contain many optional conditions such as filtering, searching, sorting, and pagination.

Specifications provide a reusable and composable way to represent those queries.

---

### Why Redis for Basket Data?

Basket operations are frequent and require low-latency access.

Redis provides fast in-memory reads and writes and is a suitable data store for temporary shopping-cart information.

---

### Why Redis Caching?

Caching prevents repeated execution of operations whose responses can safely be reused, reducing database load and API latency.

---

### Why DTOs?

Returning EF Core entities directly would tightly couple the public API contract to the persistence/domain model.

DTOs provide an explicit and safer API contract.

---

### Why AutoMapper?

AutoMapper reduces repetitive entity-to-DTO transformation code while keeping mapping configuration centralized.

---

### Why Centralized Exception Middleware?

It prevents repetitive exception-handling logic across controllers and provides consistent error responses.

---

# 📈 Main Features

* RESTful Product API
* Product CRUD operations
* Product filtering
* Product searching
* Server-side pagination
* Specification Pattern
* Repository Pattern
* Generic Repository
* Unit of Work
* Entity Framework Core
* SQL Server persistence
* Redis integration
* Redis-based basket
* Redis caching
* Attribute-based caching
* AutoMapper
* Custom Mapping Resolver
* DTO-based API contracts
* Centralized exception handling
* Dependency Injection
* Database migrations
* Database seeding
* Swagger/OpenAPI
* HTTPS redirection
* Static file serving
* Layered architecture

---

# 🚀 Possible Future Improvements

The project can be extended with additional E-Commerce features such as:

* Authentication and Authorization using JWT
* ASP.NET Core Identity
* User registration and login
* Roles and permissions
* Order management
* Checkout flow
* Payment gateway integration
* Stripe integration
* Inventory management
* Product reviews and ratings
* Wishlist functionality
* Refresh tokens
* Email notifications
* Logging with Serilog
* Health checks
* Automated unit tests
* Integration tests
* Docker support
* Docker Compose for API + SQL Server + Redis
* CI/CD pipeline
* Cloud deployment
* Rate limiting
* API versioning

---

# 📚 What This Project Demonstrates

This project demonstrates practical knowledge of backend development using the .NET ecosystem, including:

* REST API design
* Clean separation of responsibilities
* Dependency Injection
* Database abstraction
* Entity Framework Core
* Query composition
* Distributed caching
* Redis
* DTO mapping
* Error handling
* API documentation
* Scalable application structure

The project focuses not only on implementing E-Commerce functionality, but also on organizing backend code using patterns and practices commonly used in real-world ASP.NET Core applications.

---

# 👨‍💻 Author

Developed as a backend E-Commerce project using **ASP.NET Core .NET 8**, **SQL Server**, **Entity Framework Core**, **Redis**, and modern backend architecture and design patterns.
