# TUS File Upload API

A modern, scalable file upload API built with .NET 9 that implements the [TUS protocol](https://tus.io/) for resumable file uploads. This API provides robust file upload capabilities with support for large files, upload resumption, and cloud-native architecture.

## 🚀 Features

- **TUS Protocol Support**: Full implementation of the TUS 1.0.0 protocol for resumable file uploads
- **Resumable Uploads**: Continue interrupted uploads from where they left off
- **Large File Support**: Handle files of any size with efficient streaming
- **Cloud-Native Architecture**: Built with Dapr for distributed systems
- **API Versioning**: Support for multiple API versions with URL and header-based versioning
- **OpenAPI Documentation**: Auto-generated API documentation with Swagger/OpenAPI
- **Health Monitoring**: Built-in health checks and monitoring
- **Observability**: Comprehensive logging and telemetry with OpenTelemetry
- **Validation**: Request validation using FluentValidation
- **Event-Driven**: Integration with Dapr event bus for asynchronous processing

## 🛠 Technology Stack

### Core Framework
- **.NET 9** - Latest .NET framework with cutting-edge features
- **ASP.NET Core** - High-performance web framework
- **Minimal APIs** - Lightweight, high-performance API endpoints

### TUS Implementation
- **tusdotnet** (v2.10.0) - TUS protocol implementation for .NET
- **TusDiskStore** - File-based storage for uploads

### Architecture & Patterns
- **Clean Architecture** - Domain-driven design with clear separation of concerns
- **CQRS with MediatR** - Command Query Responsibility Segregation pattern
- **Repository Pattern** - Data access abstraction
- **Domain-Driven Design** - Rich domain models and aggregates

### Cloud & Distributed Systems
- **Dapr** (v1.15.4) - Distributed Application Runtime
  - **Dapr Event Bus** - Pub/Sub messaging
  - **Dapr Storage Bus** - Blob storage integration
- **Service Discovery** - Dynamic service location

### API & Documentation
- **ASP.NET Core API Versioning** (v8.1.0) - Multi-version API support
- **Scalar** (v2.5.0) - Modern API documentation UI

### Validation & Error Handling
- **FluentValidation** (v12.0.0) - Request validation
- **Custom Exception Handling** - Structured error responses
- **Problem Details** - RFC 7807 compliant error responses

### Observability & Monitoring
- **OpenTelemetry** - Distributed tracing and metrics
- **Azure Monitor** - Application monitoring and insights
- **Application Insights** - Performance monitoring
- **Console & OTLP Exporters** - Flexible telemetry export

### Development & Testing
- **FluentAssertions** - Expressive unit testing
- **HTTP Client Files** - API testing and documentation

## 📁 Project Structure

```
src/
├── Tus.Api/                    # Main API project
│   ├── Apis/                   # API endpoints
│   │   ├── Health/            # Health check endpoints
│   │   └── TusUpload/         # TUS upload endpoints
│   ├── Application/           # Application layer (CQRS, validation)
│   ├── StartupExtensions/     # Service configuration
│   └── Program.cs             # Application entry point
├── Tus.Api.Domain/            # Domain layer (entities, repositories)
│   ├── Aggregates/           # Domain aggregates
│   ├── Common/               # Shared domain concepts
│   └── Config/               # Configuration models
└── Tus.Api.Infrastructure/    # Infrastructure layer
    ├── Repositories/         # Data access implementations
    └── Services/             # External service integrations
```

## 🚀 Getting Started

### Prerequisites

- .NET 9 SDK
- Docker (for Dapr components)
- Azure subscription (for cloud services)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd tus
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure settings**
   - Update configuration values for your environment

4. **Run the application**
   ```bash
   dotnet run --project src/Tus.Api
   ```

### API Endpoints

#### TUS File Upload
- **POST** `/api/v1/file-uploads` - Upload files using TUS protocol
- **PATCH** `/api/v1/file-uploads/{fileId}` - Resume upload
- **HEAD** `/api/v1/file-uploads/{fileId}` - Get upload status
- **OPTIONS** `/api/v1/file-uploads` - Get server capabilities

#### Health Checks
- **GET** `/api/v1/health/ping` - Basic health check
- **GET** `/api/v1/health/version` - API version information

### Example Usage

#### Upload a file using TUS protocol

```bash
# Create upload
curl -X POST http://localhost:5000/api/v1/file-uploads \
  -H "Tus-Resumable: 1.0.0" \
  -H "Upload-Length: 1234567" \
  -H "Upload-Metadata: name testfile.txt,type text/plain"

# Resume upload (if interrupted)
curl -X PATCH http://localhost:5000/api/v1/file-uploads/{fileId} \
  -H "Tus-Resumable: 1.0.0" \
  -H "Upload-Offset: 1000000" \
  --data-binary @remaining_data.bin
```

## 🔧 Configuration

The API can be configured through `appsettings.json` and environment variables:

```json
{
  "TusApi": {
    "BaseOpenApiUri": "https://api.example.com"
  },
  "PubSub": {
    // Dapr pub/sub configuration
  },
  "BlobStorage": {
    // Dapr blob storage configuration
  }
}
```

## 🏗 Architecture

This API follows Clean Architecture principles with clear separation of concerns:

- **Domain Layer**: Core business logic and entities
- **Application Layer**: Use cases, commands, queries, and validation
- **Infrastructure Layer**: External integrations and data access
- **API Layer**: HTTP endpoints and request/response handling

The application uses the CQRS pattern with MediatR for handling commands and queries, providing a clean and maintainable codebase.

## 📊 Monitoring & Observability

The API includes comprehensive monitoring capabilities:

- **Health Checks**: Built-in health monitoring endpoints
- **OpenTelemetry**: Distributed tracing and metrics collection
- **Azure Monitor**: Application performance monitoring
- **Structured Logging**: Consistent log format across the application

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the terms specified in the [LICENSE](LICENSE) file.

## 🔗 Resources

- [TUS Protocol Specification](https://tus.io/protocols/resumable-upload)
- [tusdotnet Documentation](https://github.com/smatsson/tusdotnet)
- [Dapr Documentation](https://docs.dapr.io/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
