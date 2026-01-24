# 🔗 URL Shortener - Clean Architecture

A production-ready URL shortening service built with .NET 8 and Clean Architecture principles. Transform long URLs into short, memorable links with built-in analytics.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat&logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![SQLite](https://img.shields.io/badge/SQLite-3.0-003B57?style=flat&logo=sqlite)](https://www.sqlite.org/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

![URL Shortener Demo](https://via.placeholder.com/800x400/4A90E2/FFFFFF?text=URL+Shortener+API)

## ✨ Features

- 🎯 **Clean Architecture** - Separation of concerns with Domain, Application, Infrastructure, and API layers
- 🔐 **Unique Short Codes** - Cryptographically secure 7-character codes
- 📊 **Analytics** - Track click counts and last accessed timestamps
- 🚀 **High Performance** - Built with async/await patterns
- 🗄️ **SQLite Database** - Lightweight and portable
- 📝 **API Documentation** - Interactive Swagger/OpenAPI documentation
- ✅ **Input Validation** - FluentValidation for robust request validation
- 🔄 **Auto Migrations** - Database schema automatically updated on startup
- 🎨 **RESTful API** - Standard HTTP methods and status codes

## 🏗️ Architecture

This project follows **Clean Architecture** (Onion Architecture) principles:
```
┌─────────────────────────────────────────┐
│           API Layer (Web)               │
│     Controllers, Middleware, DTOs       │
├─────────────────────────────────────────┤
│        Application Layer                │
│   Use Cases, Interfaces, Validators     │
├─────────────────────────────────────────┤
│       Infrastructure Layer              │
│  EF Core, Repositories, DbContext       │
├─────────────────────────────────────────┤
│          Domain Layer (Core)            │
│     Entities, Value Objects, Rules      │
└─────────────────────────────────────────┘
```

### Project Structure
```
UrlShortener/
├── UrlShortener.Domain/              # Core business logic (no dependencies)
│   ├── Common/                        # Base entities
│   ├── Entities/                      # Domain entities
│   └── Repositories/                  # Repository interfaces
│
├── UrlShortener.Application/          # Business rules & use cases
│   ├── Common/                        # Result pattern
│   ├── DTOs/                          # Data transfer objects
│   ├── Interfaces/                    # Service interfaces
│   ├── Services/                      # Business logic
│   └── Validators/                    # FluentValidation rules
│
├── UrlShortener.Infrastructure/       # External concerns
│   ├── Persistence/                   # EF Core DbContext
│   │   └── Configurations/            # Entity configurations
│   └── Repositories/                  # Repository implementations
│
└── UrlShortener.API/                  # Presentation layer
    ├── Controllers/                   # API endpoints
    ├── Properties/                    # Launch settings
    └── Program.cs                     # App configuration
```

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [Git](https://git-scm.com/)

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/dev2298/UrlShortener.git
cd UrlShortener
```

2. **Restore dependencies**
```bash
dotnet restore
```

3. **Build the solution**
```bash
dotnet build
```

4. **Run the application**
```bash
cd UrlShortener.API
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `http://localhost:5000/swagger`

## 📖 API Documentation

### Endpoints

#### Create Short URL
```http
POST /api/url/shorten
Content-Type: application/json

{
  "originalUrl": "https://www.example.com/very/long/url"
}
```

**Response:**
```json
{
  "shortCode": "aBc1234",
  "shortUrl": "http://localhost:5000/r/aBc1234",
  "originalUrl": "https://www.example.com/very/long/url",
  "createdAt": "2026-01-25T10:30:00Z"
}
```

#### Redirect to Original URL
```http
GET /r/{shortCode}
```

**Response:** HTTP 302 Redirect to original URL

#### Get URL Statistics
```http
GET /api/url/stats/{shortCode}
```

**Response:**
```json
{
  "shortCode": "aBc1234",
  "originalUrl": "https://www.example.com/very/long/url",
  "clickCount": 42,
  "createdAt": "2026-01-25T10:30:00Z",
  "lastAccessedAt": "2026-01-25T15:45:00Z"
}
```

#### Get URL Details
```http
GET /api/url/{shortCode}
```

**Response:**
```json
{
  "shortCode": "aBc1234",
  "shortUrl": "http://localhost:5000/r/aBc1234",
  "originalUrl": "https://www.example.com/very/long/url",
  "createdAt": "2026-01-25T10:30:00Z"
}
```

## 💡 Usage Examples

### cURL
```bash
# Create a short URL
curl -X POST http://localhost:5000/api/url/shorten \
  -H "Content-Type: application/json" \
  -d '{"originalUrl": "https://github.com/dev2298"}'

# Get statistics
curl http://localhost:5000/api/url/stats/aBc1234

# Test redirect
curl -L http://localhost:5000/r/aBc1234
```

### C# HttpClient
```csharp
using System.Net.Http.Json;

var client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };

// Create short URL
var request = new { originalUrl = "https://github.com/dev2298" };
var response = await client.PostAsJsonAsync("/api/url/shorten", request);
var result = await response.Content.ReadFromJsonAsync<ShortenedUrlResponse>();

Console.WriteLine($"Short URL: {result.ShortUrl}");
```

### JavaScript/Fetch
```javascript
// Create short URL
const response = await fetch('http://localhost:5000/api/url/shorten', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ originalUrl: 'https://github.com/dev2298' })
});

const data = await response.json();
console.log('Short URL:', data.shortUrl);
```

## 🛠️ Technologies & Design Patterns

### Technologies
- **.NET 8** - Latest .NET framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM
- **SQLite** - Embedded database
- **FluentValidation** - Input validation
- **Swagger/OpenAPI** - API documentation

### Design Patterns & Principles
- ✅ Clean Architecture (Onion Architecture)
- ✅ Repository Pattern
- ✅ Unit of Work Pattern
- ✅ Dependency Injection
- ✅ Result Pattern (Railway-Oriented Programming)
- ✅ SOLID Principles
- ✅ Domain-Driven Design (DDD)
- ✅ Separation of Concerns
- ✅ Async/Await Pattern

## 📊 Database Schema
```sql
CREATE TABLE ShortenedUrls (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ShortCode TEXT NOT NULL UNIQUE,
    OriginalUrl TEXT NOT NULL,
    ClickCount INTEGER NOT NULL DEFAULT 0,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT,
    LastAccessedAt TEXT
);

CREATE UNIQUE INDEX IX_ShortenedUrls_ShortCode ON ShortenedUrls(ShortCode);
```

## 🧪 Testing
```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true
```

## 🔧 Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=urlshortener.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## 📈 Performance

- **Short Code Generation**: O(1) - Cryptographically secure random generation
- **URL Lookup**: O(1) - Database index on ShortCode
- **Duplicate Detection**: O(1) - Database query with index
- **Database**: SQLite with proper indexing for fast lookups

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Devansh Srivastava**

- GitHub: [@dev2298](https://github.com/dev2298)
- LinkedIn: [Connect with me](https://linkedin.com/in/dev2298)

## 🙏 Acknowledgments

- Clean Architecture by Robert C. Martin
- .NET Community for excellent documentation
- Entity Framework Core team

## 📞 Support

If you have any questions or run into issues, please open an issue on GitHub.

---

⭐ If you found this project helpful, please give it a star!

Made with ❤️ by Devansh Srivastava
