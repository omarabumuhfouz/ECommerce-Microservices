# 🚀 Enterprise E-Commerce Microservices (.NET 9)

[![.NET 9](https://img.shields.io/badge/.NET-9.0-512bd4?logo=dotnet)](https://dotnet.microsoft.com/download)
[![Microservices](https://img.shields.io/badge/Architecture-Microservices-orange)](#)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A highly scalable, cloud-native distributed system built with **.NET 9**. This project is a comprehensive showcase of modern software engineering, demonstrating advanced implementation of **Domain-Driven Design (DDD)**, **Vertical Slice Architecture (VSA)**, and **Functional Programming** concepts.

> **Note:** Engineered for maintainability and high performance using **Minimal APIs** and **Railway Oriented Programming (ROP)** to ensure clean, predictable, and robust code.

---

## 🏗️ Architectural Foundations

The architecture is built on four core pillars, prioritizing cohesion and loose coupling:

*   **Vertical Slice Architecture (VSA):** Departing from traditional layered architectures, features are encapsulated into independent "slices." Each slice contains its own API endpoint, business logic, and data access.
*   **Domain-Driven Design (DDD) & Specification Pattern:** Business logic is strictly encapsulated using rich domain models and the **Specification Pattern**. This replaces the traditional Repository Pattern, allowing for reusable, strongly-typed query logic and business rules while maintaining a clean, thin data access layer.
*   **CQRS Pattern:** Strict separation of Commands (state mutations) and Queries (data retrieval) for optimized data access.
*   **Railway Oriented Programming (ROP):** Utilizes a **Result Pattern** to manage flow, treating failures as first-class citizens and eliminating nested `try-catch` blocks.

---

## 🛠️ Technology Stack

| Category | Technology |
| :--- | :--- |
| **Framework** | .NET 9 (ASP.NET Core Minimal APIs) |
| **Database** | Microsoft SQL Server & Entity Framework Core |
| **Message Broker** | RabbitMQ integrated with MassTransit |
| **Internal Comm** | gRPC (High-performance synchronous) |
| **API Gateway** | Ocelot (External routing & aggregation) |
| **Observability** | Standardized Health Checks (Liveness/Readiness) |
| **Resilience** | Transactional Outbox Pattern |

---

## 🛰️ Microservices Ecosystem

### 1. 🛡️ Auth Service
The security gateway for the system.
*   **Responsibilities:** Identity management, user registration, and issuing JWT tokens for secure service-to-service and client communication.

### 2. 📋 Order Service
The orchestrator of the purchasing workflow.
*   **Responsibilities:** Managing the order lifecycle, state transitions (Pending, Paid, Shipped), and enforcing complex business rules during checkout.
*   **Pattern:** Heavy use of DDD Aggregates to maintain order integrity.

### 3. 📦 Product Service
The core catalog engine built with strict **VSA**.
*   **Responsibilities:** Product lifecycle, metadata, and catalog presentation.
*   **Current State:** Manages inventory adjustments (scheduled for migration to a dedicated Inventory Service).

### 4. 🛒 Shopping Cart Service
Manages active user sessions and pre-checkout state.
*   **Responsibilities:** High-speed item management and calculating running totals.

### 5. 📧 Email Service
The primary notification handler for the ecosystem.
*   **Responsibilities:** Consuming integration events (like `OrderCreated` or `UserRegistered`) to send automated notifications to customers.

### 6. ⭐ Feedback Service
A decoupled service managing customer interactions.
*   **Responsibilities:** Processing product ratings and reviews.
*   **Integration:** Publishes integration events to update aggregate ratings asynchronously.

### 7. 🔄 Cancellation Service
A specialized service orchestrating distributed rollbacks.
*   **Responsibilities:** Reversing orders and restoring system state when operations are aborted.

### 8. 👤 Customer Service
The source of truth for user identities and profiles.
*   **Responsibilities:** Managing customer details, preferences, and account states.


### 9. ⚙️ Infrastructure: SharedKernel & Contracts
*   **SharedKernel:** Centralized logic for `Result<T>`, ROP extensions, and DDD primitives.
*   **Contracts:** Shared schema registry for **gRPC Protobuf** definitions and messaging contracts.


### 8. 🛡️ API Gateway (Ocelot) & Infrastructure
*   **Gateway:** Unified entry point for request routing and downstream aggregation.
*   **SharedKernel:** Centralized logic for `Result<T>` and ROP extensions.
*   **Contracts:** Shared schema registry for **gRPC Protobuf** definitions.

---

## 🩺 Resilience & Full-Stack Observability

The system implements a modern observability stack to monitor distributed health:

*   **Structured Logging (Serilog & Seq):** All services use Serilog for structured logging, providing deep insights into application behavior. Logs are centralized and visualized via the **Seq UI**.
*   **Distributed Tracing (OpenTelemetry & Grafana):** Basic OpenTelemetry setup is implemented across services. Traces and metrics are visualized in **Grafana**, allowing for request tracking across gRPC and RabbitMQ boundaries.
*   **Health Checks (`/health`):** Every service exposes liveness and readiness endpoints, monitoring the status of **SQL Server** and **RabbitMQ** connections.
---

## 🔄 Functional Flow Example (ROP)

The system uses a functional approach for business logic to keep feature slices clean:

```csharp
public async Task<Result<Guid>> Handle(CreateProductCommand command)
{
    return await Result.Success(command)
        .Bind(cmd => ValidateBusinessRules(cmd))
        .Bind(cmd => MapToDomainEntity(cmd))
        .Bind(entity => SaveToDatabaseWithOutboxAsync(entity));
}


---

## 🚧 Development Roadmap

To achieve full production readiness and evolve the system's capabilities, the following milestones are currently being addressed:

- [x] **🔐 Auth Service:** Core identity and JWT implementation completed.
- [x] **📊 Basic Observability:** **Seq** and **Grafana** UIs are operational with initial **OpenTelemetry** instrumentation.
- [ ] **📧 Email Service (Active Development):** 
  - Implementing robust event consumers to handle system notifications (Order confirmations, Welcome emails).
  - Ensuring high reliability and retry policies via MassTransit.
- [ ] **📦 Inventory Service:** 
  - Extracting stock management logic from the Product Service into a dedicated bounded context.
  - Implementing high-concurrency handling for inventory reservations.
- [ ] **⚡ Performance Optimization (Dapper):** 
  - Introducing **Dapper** for high-speed, read-only models within the CQRS pipeline to bypass EF Core overhead for complex queries.
- [ ] **🔔 Real-time Features (SignalR):** 
  - Integrating **SignalR** to provide live updates for order status changes and real-time inventory alerts to the frontend.
- [ ] **🧪 Comprehensive Testing:** 
  - Implementing **xUnit** for Domain logic and **WebApplicationFactory** for end-to-end integration tests.

---
*Developed with a focus on Clean Architecture, Software Craftsmanship, and Scalable Systems.*