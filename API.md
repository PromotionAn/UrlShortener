# API Documentation

## Base URL
```
http://localhost:5000
```

## Authentication

Currently, no authentication is required.

## Endpoints

### 1. Create Short URL

Creates a shortened URL from a long URL.

**Endpoint:** `POST /api/url/shorten`

**Request Body:**
```json
{
  "originalUrl": "string (required, max 2048 characters, must be valid URL)"
}
```

**Response:** `200 OK`
```json
{
  "shortCode": "string",
  "shortUrl": "string",
  "originalUrl": "string",
  "createdAt": "datetime"
}
```

**Error Responses:**
- `400 Bad Request` - Invalid URL format
- `500 Internal Server Error` - Server error

**Example:**
```bash
curl -X POST http://localhost:5000/api/url/shorten \
  -H "Content-Type: application/json" \
  -d '{"originalUrl": "https://example.com/very/long/url"}'
```

---

### 2. Get URL Details

Retrieves details of a shortened URL without incrementing click count.

**Endpoint:** `GET /api/url/{shortCode}`

**Response:** `200 OK`
```json
{
  "shortCode": "string",
  "shortUrl": "string",
  "originalUrl": "string",
  "createdAt": "datetime"
}
```

**Error Responses:**
- `404 Not Found` - Short code doesn't exist

**Example:**
```bash
curl http://localhost:5000/api/url/aBc1234
```

---

### 3. Get URL Statistics

Retrieves statistics for a shortened URL including click count.

**Endpoint:** `GET /api/url/stats/{shortCode}`

**Response:** `200 OK`
```json
{
  "shortCode": "string",
  "originalUrl": "string",
  "clickCount": "integer",
  "createdAt": "datetime",
  "lastAccessedAt": "datetime | null"
}
```

**Error Responses:**
- `404 Not Found` - Short code doesn't exist

**Example:**
```bash
curl http://localhost:5000/api/url/stats/aBc1234
```

---

### 4. Redirect to Original URL

Redirects to the original URL and increments click count.

**Endpoint:** `GET /r/{shortCode}`

**Response:** `302 Found`
- Location header contains original URL

**Error Responses:**
- `404 Not Found` - Short code doesn't exist

**Example:**
```bash
curl -L http://localhost:5000/r/aBc1234
```

## Rate Limiting

Currently, no rate limiting is implemented. Consider implementing in production.

## Error Format

All errors return JSON in this format:
```json
{
  "message": "Error description"
}
```

## Data Validation

- URLs must be valid HTTP/HTTPS URLs
- Maximum URL length: 2048 characters
- Short codes are 7 characters (alphanumeric)

## Behavior Notes

- Duplicate URLs return the existing short code
- Short codes are case-sensitive
- Click count increments only on `/r/{shortCode}` endpoint
- Database automatically created on first run
