# Vedic API - Book Management System

Enterprise-grade .NET Core Web API for managing Vedic books and content.

## 🏗️ Architecture

This API follows **Clean Architecture** principles with clear separation of concerns:

```
VedicAPI.API/
├── Controllers/          # API endpoints
├── Services/            # Business logic layer
│   ├── Interfaces/
│   └── BookService.cs
├── Repositories/        # Data access layer
│   ├── Interfaces/
│   └── BookRepository.cs
├── Models/              # Domain models
│   ├── DTOs/           # Data Transfer Objects
│   └── Common/         # Shared models
└── Database/           # SQL scripts
```

## 🚀 Technologies

- **.NET 8.0** - Latest .NET framework
- **Dapper** - Lightweight ORM for high-performance data access
- **SQL Server** - Enterprise database
- **Swagger/OpenAPI** - API documentation
- **Dependency Injection** - Built-in DI container

## 📋 Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 or VS Code

## 🔧 Setup Instructions

### 1. Database Setup

#### Initial Setup (New Installation)

Run the SQL script to create the database and tables:

```bash
# Using SQL Server Management Studio (SSMS)
# Open: VedicAPI.API/Database/CreateDatabase.sql
# Execute the script

# Or using sqlcmd
sqlcmd -S localhost -i "VedicAPI.API/Database/CreateDatabase.sql"
```

#### Migration to Chapter-Based System (Existing Installation)

If you already have the database and want to migrate to the chapter-based system:

```bash
# Using SQL Server Management Studio (SSMS)
# Open: VedicAPI.API/Database/MigrateToChapters.sql
# Execute the script

# Or using sqlcmd
sqlcmd -S localhost -i "VedicAPI.API/Database/MigrateToChapters.sql"
```

**Note:** The migration script will:
- Drop the `Content` column from the `Books` table
- Add `TotalChapters` and `CoverImageUrl` columns
- Create the `BookChapters` table
- Insert sample chapter data for testing

### 2. Update Connection String

Edit `appsettings.json` or `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=VedicDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

For SQL Server Authentication:
```json
"DefaultConnection": "Server=localhost;Database=VedicDB;User Id=your_user;Password=your_password;TrustServerCertificate=True;"
```

### 3. Restore NuGet Packages

```bash
cd VedicAPI.API
dotnet restore
```

### 4. Build the Project

```bash
dotnet build
```

### 5. Run the API

```bash
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:7xxx`
- HTTP: `http://localhost:5xxx`
- Swagger UI: `https://localhost:7xxx/swagger`

## 📚 API Endpoints

### Books

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/books` | Get all books |
| GET | `/api/books/active` | Get active books only |
| GET | `/api/books/{id}` | Get book by ID |
| GET | `/api/books/category/{category}` | Get books by category |
| GET | `/api/books/search?searchTerm={term}` | Search books |
| POST | `/api/books` | Create a new book |
| PUT | `/api/books/{id}` | Update a book |
| DELETE | `/api/books/{id}` | Delete a book |

### Book Chapters

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/books/{id}/chapters` | Get book with all chapters (summaries) |
| GET | `/api/books/{bookId}/chapters/{chapterNumber}` | Get specific chapter with content |
| GET | `/api/books/{bookId}/chapters/{chapterNumber}/next` | Get next chapter |
| GET | `/api/books/{bookId}/chapters/{chapterNumber}/previous` | Get previous chapter |

### Chapters Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/chapters/{id}` | Get chapter by ID |
| POST | `/api/chapters/books/{bookId}` | Create a new chapter |
| PUT | `/api/chapters/{id}` | Update a chapter |
| DELETE | `/api/chapters/{id}` | Delete a chapter |

## 📝 Request/Response Examples

### Create Book (POST /api/books)

**Request Body:**
```json
{
  "title": "Sushruta Samhita",
  "author": "Acharya Sushruta",
  "description": "Ancient text on surgery",
  "coverImageUrl": "https://example.com/cover.jpg",
  "category": "Ayurveda",
  "language": "Sanskrit",
  "publicationYear": 600,
  "isbn": null
}
```

**Response:**
```json
{
  "success": true,
  "message": "Book created successfully",
  "data": {
    "id": 1,
    "title": "Sushruta Samhita",
    "author": "Acharya Sushruta",
    "description": "Ancient text on surgery",
    "coverImageUrl": "https://example.com/cover.jpg",
    "totalChapters": 0,
    "category": "Ayurveda",
    "language": "Sanskrit",
    "publicationYear": 600,
    "isbn": null,
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": null,
    "isActive": true
  },
  "errors": null
}
```

### Get Book with Chapters (GET /api/books/{id}/chapters)

**Response:**
```json
{
  "success": true,
  "message": "Book with chapters retrieved successfully",
  "data": {
    "id": 1,
    "title": "Sushruta Samhita",
    "author": "Acharya Sushruta",
    "description": "Ancient text on surgery",
    "totalChapters": 3,
    "chapters": [
      {
        "id": 1,
        "chapterNumber": 1,
        "chapterTitle": "Introduction to Shalya Tantra",
        "chapterSubtitle": "The Ancient Science of Surgery",
        "summary": "Introduction to the ancient science of surgery...",
        "readingTimeMinutes": 5,
        "isActive": true
      },
      {
        "id": 2,
        "chapterNumber": 2,
        "chapterTitle": "Surgical Instruments",
        "chapterSubtitle": "Yantra Shastra - The 101 Instruments",
        "summary": "Detailed description of surgical instruments...",
        "readingTimeMinutes": 4,
        "isActive": true
      }
    ]
  },
  "errors": null
}
```

### Get Chapter (GET /api/books/{bookId}/chapters/{chapterNumber})

**Response:**
```json
{
  "success": true,
  "message": "Chapter retrieved successfully",
  "data": {
    "id": 1,
    "bookId": 1,
    "bookTitle": "Sushruta Samhita",
    "bookAuthor": "Acharya Sushruta",
    "chapterNumber": 1,
    "chapterTitle": "Introduction to Shalya Tantra",
    "chapterSubtitle": "The Ancient Science of Surgery",
    "contentHtml": "<div class=\"chapter-content\"><h2>Historical Context</h2><p>The Sushruta Samhita...</p></div>",
    "summary": "Introduction to the ancient science of surgery...",
    "wordCount": 450,
    "readingTimeMinutes": 5,
    "hasPreviousChapter": false,
    "hasNextChapter": true,
    "previousChapterId": null,
    "nextChapterId": 2
  },
  "errors": null
}
```

### Create Chapter (POST /api/chapters/books/{bookId})

**Request Body:**
```json
{
  "chapterNumber": 1,
  "chapterTitle": "Introduction to Shalya Tantra",
  "chapterSubtitle": "The Ancient Science of Surgery",
  "contentHtml": "<div class=\"chapter-content\"><h2>Historical Context</h2><p>Content here...</p></div>",
  "summary": "Introduction to the ancient science of surgery...",
  "wordCount": 450,
  "readingTimeMinutes": 5,
  "displayOrder": 1
}
```

**Response:**
```json
{
  "success": true,
  "message": "Chapter created successfully",
  "data": {
    "id": 1,
    "bookId": 1,
    "chapterNumber": 1,
    "chapterTitle": "Introduction to Shalya Tantra",
    // ... full chapter details
  },
  "errors": null
}
```

### Get All Books (GET /api/books)

**Response:**
```json
{
  "success": true,
  "message": "Books retrieved successfully",
  "data": [
    {
      "id": 1,
      "title": "Sushruta Samhita",
      "author": "Acharya Sushruta",
      // ... other fields
    }
  ],
  "errors": null
}
```

## 🔒 CORS Configuration

The API is configured to accept requests from:
- `http://localhost:3000` (React default)
- `http://localhost:3001`

To add more origins, update `Program.cs`:

```csharp
policy.WithOrigins("http://localhost:3000", "http://your-domain.com")
```

## 🧪 Testing

### Using Swagger UI

1. Run the API
2. Navigate to `https://localhost:7xxx/swagger`
3. Test endpoints directly from the browser

### Using curl

```bash
# Get all books
curl -X GET "https://localhost:7xxx/api/books" -k

# Create a book
curl -X POST "https://localhost:7xxx/api/books" \
  -H "Content-Type: application/json" \
  -d '{"title":"Test Book","author":"Test Author","content":"Test Content"}' \
  -k
```

## 📦 Project Structure Details

### Models
- **Book.cs** - Main entity representing a book
- **BookChapter.cs** - Entity representing a book chapter
- **BookCreateDto.cs** - DTO for creating books
- **BookUpdateDto.cs** - DTO for updating books
- **BookResponseDto.cs** - DTO for API responses
- **BookWithChaptersDto.cs** - DTO for book with chapter summaries
- **ChapterSummaryDto.cs** - DTO for chapter summary (without content)
- **ChapterDetailDto.cs** - DTO for full chapter with HTML content
- **ChapterCreateDto.cs** - DTO for creating chapters
- **ChapterUpdateDto.cs** - DTO for updating chapters
- **ApiResponse.cs** - Generic response wrapper

### Repository Pattern
- **IBookRepository** - Interface defining book data operations
- **BookRepository** - Dapper implementation for books
- **IBookChapterRepository** - Interface defining chapter data operations
- **BookChapterRepository** - Dapper implementation for chapters

### Service Layer
- **IBookService** - Interface for book business logic
- **BookService** - Implementation with validation and mapping
- **IBookChapterService** - Interface for chapter business logic
- **BookChapterService** - Implementation with chapter operations and navigation

### Controllers
- **BooksController** - RESTful API endpoints for books and chapter navigation
- **ChaptersController** - RESTful API endpoints for chapter management

## 🔍 Features

✅ Full CRUD operations for books and chapters
✅ Chapter-based book content management
✅ HTML content storage for rich formatting
✅ Chapter navigation (next/previous)
✅ Search functionality
✅ Category filtering
✅ Automatic word count and reading time calculation
✅ Proper error handling
✅ Logging
✅ Input validation
✅ Swagger documentation
✅ CORS support
✅ Repository pattern
✅ Service layer
✅ DTOs for clean API contracts
✅ Generic API response wrapper

## 🚀 Deployment

### IIS Deployment

1. Publish the project:
```bash
dotnet publish -c Release -o ./publish
```

2. Create an IIS application pool
3. Point to the published folder
4. Update connection string in `appsettings.json`

### Docker (Optional)

Create a `Dockerfile`:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["VedicAPI.API/VedicAPI.API.csproj", "VedicAPI.API/"]
RUN dotnet restore "VedicAPI.API/VedicAPI.API.csproj"
COPY . .
WORKDIR "/src/VedicAPI.API"
RUN dotnet build "VedicAPI.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VedicAPI.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VedicAPI.API.dll"]
```

## 📄 License

This project is part of the Vedic AI educational platform.

## 👥 Support

For issues or questions, please contact the Vedic AI team.

