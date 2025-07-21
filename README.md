# YourInvoice Digital Platform

[![.NET](https://img.shields.io/badge/.NET-7.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![Azure](https://img.shields.io/badge/Azure-Cloud-0078D4?style=flat&logo=microsoft-azure)](https://azure.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Build Status](https://img.shields.io/badge/Build-Passing-brightgreen.svg)](https://github.com/yourusername/yourInvoice)

A comprehensive digital invoicing platform built with .NET 7, featuring a modern microservices architecture and Clean Architecture principles. This solution provides a robust foundation for invoice management, user linking processes, and integration with external systems.

## 🏗️ Architecture Overview

This solution follows **Clean Architecture** and **Domain-Driven Design (DDD)** principles, organized into distinct bounded contexts:

```
├── 📦 Common/                    # Shared libraries and utilities
├── 🧾 Offer/                     # Invoice and offer management
├── 🔗 Link/                      # User onboarding and KYC processes  
├── ⚡ Functions/                 # Azure Functions for background processing
├── 🚀 Pipelines/                 # CI/CD pipeline definitions
└── 📋 Documentation/             # Project documentation
```

## 🛠️ Technology Stack

### Backend Technologies
- **.NET 7.0** - Primary framework
- **ASP.NET Core Web API** - RESTful API endpoints
- **Entity Framework Core** - ORM and data access
- **MediatR** - CQRS pattern implementation
- **AutoMapper** - Object mapping
- **FluentValidation** - Input validation

### Cloud & Infrastructure
- **Azure Functions** - Serverless computing
- **Azure Service Bus** - Message queuing
- **Azure SQL Database** - Primary data storage
- **Azure B2C** - Identity and authentication
- **Application Insights** - Monitoring and telemetry

### Testing & Quality
- **xUnit** - Unit testing framework
- **FluentAssertions** - Test assertions
- **Moq** - Mocking framework
- **SonarQube** - Code quality analysis

## 🚀 Getting Started

### Prerequisites

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/) (for deployment)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) or Azure SQL Database

### Local Development Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/yourInvoice.git
   cd yourInvoice
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Update configuration**
   - Copy `appsettings.Development.json.template` to `appsettings.Development.json`
   - Update connection strings and configuration values
   - Ensure `local.settings.json` files are configured for Azure Functions

4. **Run database migrations**
   ```bash
   dotnet ef database update --project Offer/src/Infrastructure
   dotnet ef database update --project Link/src/YourInvoice.Link
   ```

5. **Build and run**
   ```bash
   dotnet build
   dotnet run --project Offer/src/Web.API
   ```

The API will be available at `https://localhost:7212`

## 📊 Project Structure

### Core Modules

#### 🧾 Offer Module
Manages the complete invoice lifecycle including creation, approval, and processing.

**Key Features:**
- Invoice creation and management
- Offer processing workflows
- Document generation and storage
- Integration with external systems

**Architecture:**
```
Offer/
├── src/
│   ├── Domain/           # Business entities and rules
│   ├── Application/      # Use cases and commands/queries
│   ├── Infrastructure/   # Data persistence and external services
│   └── Web.API/         # HTTP endpoints and controllers
└── tests/               # Unit and integration tests
```

#### 🔗 Link Module  
Handles user onboarding, KYC (Know Your Customer) processes, and account linking.

**Key Features:**
- User registration and validation
- Document upload and verification
- Compliance checks (SAGRILAFT)
- Digital signature integration

#### ⚡ Azure Functions
Background processing services for asynchronous operations.

**Functions:**
- **DIAN Integration** - Tax authority integrations
- **FTP Factoring** - File processing and factoring services

## 🔧 Configuration

### Environment Variables

| Variable | Description | Required |
|----------|-------------|----------|
| `ConnectionStrings:SqlServer` | SQL Server connection string | Yes |
| `AzureAdB2C:TenantId` | Azure B2C tenant identifier | Yes |
| `AzureAdB2C:ClientId` | Azure B2C application ID | Yes |
| `ApplicationInsights:ConnectionString` | Application Insights connection | Yes |
| `ServiceBusConnectionString` | Azure Service Bus connection | Yes |

### Security Configuration

All sensitive configuration values have been sanitized for public release. Use the following placeholders:

- Database connections: `__sanitized_sql_server__`
- API keys: `__sanitized_api_key__`
- Secrets: `__sanitized_secret__`

## 🧪 Testing

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test Common/tests/UnitTests/Application.Customers.UnitTests/
```

### Test Structure

- **Unit Tests** - Business logic and domain validation
- **Integration Tests** - API endpoints and database operations
- **Architecture Tests** - Dependency and structure validation

## 📦 Deployment

### Azure Deployment

The project includes Azure DevOps pipelines for automated deployment:

1. **Build Pipeline** - Compiles, tests, and packages the application
2. **Release Pipeline** - Deploys to different environments (dev/test/prod)

### Local Docker Deployment

```bash
# Build Docker images
docker-compose build

# Run the application
docker-compose up -d
```

## 🔍 Monitoring and Observability

- **Application Insights** - Performance monitoring and telemetry
- **Structured Logging** - Comprehensive logging with Serilog
- **Health Checks** - API and dependency health monitoring
- **Metrics** - Custom business metrics and KPIs

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Code Standards

- Follow C# naming conventions
- Write unit tests for new features
- Ensure SonarQube quality gates pass
- Update documentation as needed

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

- Create an [Issue](https://github.com/yourusername/yourInvoice/issues) for bug reports
- Start a [Discussion](https://github.com/yourusername/yourInvoice/discussions) for questions
- Check the [Wiki](https://github.com/yourusername/yourInvoice/wiki) for detailed documentation

## 🏆 Acknowledgments

- Built with [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) principles
- Inspired by [Domain-Driven Design](https://domainlanguage.com/ddd/) practices
- Uses [Microsoft's .NET Application Architecture Guides](https://dotnet.microsoft.com/learn/dotnet/architecture-guides)

---

<p align="center">
  <strong>YourInvoice Digital Platform</strong><br>
  A modern, scalable invoicing solution built with .NET and Azure
</p>
