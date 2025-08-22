# API Reference - REDZAuth API

## 📋 Overview

This document provides a comprehensive reference for all API endpoints, request/response formats, and usage examples.

## 🔌 Base URL

```
https://localhost:7001
```

## 📚 API Documentation

Interactive API documentation is available at: `https://localhost:7001/swagger`

## 🔐 Authentication Endpoints

### Register User

**Endpoint:** `POST /api/auth/register`

**Description:** Register a new user account with license key validation.

**Request Body:**
```json
{
  "username": "string",
  "password": "string",
  "key": "string",
  "hwid": "string",
  "ip": "string"
}
```

**Parameters:**
- `username` (required) - Unique username for the account
- `password` (required) - User password (will be hashed)
- `key` (required) - Valid license key for registration
- `hwid` (required) - Hardware ID for device binding
- `ip` (required) - IP address for tracking

**Response (Success - 200):**
```json
{
  "message": "Account Created Successfully",
  "username": "testuser",
  "plan": "mensal",
  "expiration": "2024-02-15T10:30:00Z"
}
```

**Response (Error - 400):**
```json
{
  "message": "Failed to Authenticate: Username already exists"
}
```

**Example:**
```bash
curl -X POST "https://localhost:7001/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "password123",
    "key": "MENSAL-ABCD-1234",
    "hwid": "device-hwid-here",
    "ip": "192.168.1.1"
  }'
```

### Login User

**Endpoint:** `POST /api/auth/login`

**Description:** Authenticate user and validate HWID binding.

**Request Body:**
```json
{
  "username": "string",
  "password": "string",
  "hwid": "string",
  "ip": "string"
}
```

**Parameters:**
- `username` (required) - Registered username
- `password` (required) - User password
- `hwid` (required) - Hardware ID for validation
- `ip` (required) - IP address for tracking

**Response (Success - 200):**
```json
{
  "message": "Login Successful",
  "username": "testuser",
  "plan": "mensal",
  "expiration": "2024-02-15T10:30:00Z"
}
```

**Response (Error - 400):**
```json
{
  "message": "Failed to Authenticate: Invalid Password"
}
```

**Example:**
```bash
curl -X POST "https://localhost:7001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "password123",
    "hwid": "device-hwid-here",
    "ip": "192.168.1.1"
  }'
```

## 🔑 License Management Endpoints

### Generate License Key

**Endpoint:** `POST /api/license/generate`

**Description:** Generate a new license key with specified plan.

**Query Parameters:**
- `plan` (required) - License plan type (mensal, trimestral, anual, lifetime)

**Response (Success - 200):**
```json
{
  "message": "License Key Created Successfully",
  "key": "MENSAL-ABCD-1234",
  "plan": "mensal",
  "expiration": "2024-02-15T10:30:00Z"
}
```

**Example:**
```bash
curl -X POST "https://localhost:7001/api/license/generate?plan=mensal"
```

### Create Custom License Key

**Endpoint:** `POST /api/license/custom`

**Description:** Create a license key with custom value.

**Query Parameters:**
- `customKey` (required) - Custom license key value
- `plan` (required) - License plan type

**Response (Success - 200):**
```json
{
  "message": "Custom License Key Created Successfully",
  "key": "CUSTOM-KEY-123",
  "plan": "mensal"
}
```

**Example:**
```bash
curl -X POST "https://localhost:7001/api/license/custom?customKey=CUSTOM-KEY-123&plan=mensal"
```

### Alternative License Generation

**Endpoint:** `POST /api/key/generate`

**Description:** Alternative endpoint for generating license keys.

**Query Parameters:**
- `plan` (required) - License plan type

**Response (Success - 200):**
```json
{
  "message": "Key generated successfully",
  "key": "MENSAL-EFGH-5678"
}
```

**Example:**
```bash
curl -X POST "https://localhost:7001/api/key/generate?plan=anual"
```

### Alternative Custom License

**Endpoint:** `POST /api/key/custom`

**Description:** Alternative endpoint for creating custom license keys.

**Query Parameters:**
- `plan` (required) - License plan type
- `customKey` (required) - Custom license key value

**Response (Success - 200):**
```json
{
  "message": "Custom key created successfully",
  "key": "CUSTOM-KEY-456"
}
```

**Example:**
```bash
curl -X POST "https://localhost:7001/api/key/custom?plan=lifetime&customKey=LIFETIME-KEY-789"
```

### List License Keys

**Endpoint:** `GET /api/key/list`

**Description:** List license keys with optional filtering.

**Query Parameters:**
- `plan` (optional) - Filter by plan type
- `used` (optional) - Filter by usage status (true/false)
- `fromDate` (optional) - Filter by creation date (YYYY-MM-DD)
- `toDate` (optional) - Filter by creation date (YYYY-MM-DD)

**Response (Success - 200):**
```json
[
  {
    "key": "MENSAL-ABCD-1234",
    "plan": "mensal",
    "usedBy": "testuser",
    "expiration": "2024-02-15T10:30:00Z",
    "createdAt": "2024-01-15T10:30:00Z"
  },
  {
    "key": "ANUAL-EFGH-5678",
    "plan": "anual",
    "usedBy": null,
    "expiration": "2025-01-15T10:30:00Z",
    "createdAt": "2024-01-15T10:30:00Z"
  }
]
```

**Example:**
```bash
curl -X GET "https://localhost:7001/api/key/list?plan=mensal&used=false"
```

## 👨‍💼 Admin Endpoints

### Reset HWID

**Endpoint:** `POST /api/admin/reset-hwid`

**Description:** Reset HWID binding for a user account.

**Query Parameters:**
- `username` (required) - Username to reset HWID for

**Response (Success - 200):**
```json
{
  "message": "HWID Reset Successfully"
}
```

**Response (Error - 400):**
```json
{
  "message": "Failed to Reset HWID: User not found"
}
```

**Example:**
```bash
curl -X POST "https://localhost:7001/api/admin/reset-hwid?username=testuser"
```

### Alternative HWID Reset

**Endpoint:** `POST /api/hwid/reset`

**Description:** Alternative endpoint for resetting HWID.

**Request Body:**
```json
{
  "username": "string"
}
```

**Response (Success - 200):**
```
HWID Reset Successfully
```

**Example:**
```bash
curl -X POST "https://localhost:7001/api/hwid/reset" \
  -H "Content-Type: application/json" \
  -d '{"username": "testuser"}'
```

## 🚫 Blacklist Management

### Ban User/IP/HWID

**Endpoint:** `POST /api/ban`

**Description:** Ban user, IP address, or HWID.

**Request Body:**
```json
{
  "username": "string",
  "ip": "string",
  "hwid": "string",
  "banUser": true,
  "banIP": false,
  "banHWID": false,
  "reason": "string"
}
```

**Parameters:**
- `username` (optional) - Username to ban
- `ip` (optional) - IP address to ban
- `hwid` (optional) - HWID to ban
- `banUser` (required) - Whether to ban the username
- `banIP` (required) - Whether to ban the IP address
- `banHWID` (required) - Whether to ban the HWID
- `reason` (required) - Reason for the ban

**Response (Success - 200):**
```
Ban registered successfully.
```

**Example:**
```bash
curl -X POST "https://localhost:7001/api/ban" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "banUser": true,
    "banIP": false,
    "banHWID": false,
    "reason": "Violation of terms of service"
  }'
```

### Unban User/IP/HWID

**Endpoint:** `POST /api/unban`

**Description:** Remove user, IP address, or HWID from blacklist.

**Request Body:**
```json
{
  "username": "string",
  "ip": "string",
  "hwid": "string",
  "unbanUser": true,
  "unbanIP": false,
  "unbanHWID": false,
  "reason": "string"
}
```

**Parameters:**
- `username` (optional) - Username to unban
- `ip` (optional) - IP address to unban
- `hwid` (optional) - HWID to unban
- `unbanUser` (required) - Whether to unban the username
- `unbanIP` (required) - Whether to unban the IP address
- `unbanHWID` (required) - Whether to unban the HWID
- `reason` (required) - Reason for the unban

**Response (Success - 200):**
```
Unban completed successfully.
```

**Example:**
```bash
curl -X POST "https://localhost:7001/api/unban" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "unbanUser": true,
    "unbanIP": false,
    "unbanHWID": false,
    "reason": "Appeal approved"
  }'
```

## 📊 Status Endpoints

### API Status

**Endpoint:** `GET /apistatus`

**Description:** Get API health status and system information.

**Response (Success - 200):**
```html
<!DOCTYPE html>
<html>
<head>
    <title>Status - REDZAuth API</title>
    <!-- Status page HTML content -->
</head>
<body>
    <!-- Status information -->
</body>
</html>
```

**Example:**
```bash
curl -X GET "https://localhost:7001/apistatus"
```

## 📝 Error Responses

### Common Error Codes

**400 Bad Request**
```json
{
  "message": "Username, password and key are required."
}
```

**401 Unauthorized**
```json
{
  "message": "Failed to Authenticate: Invalid Password"
}
```

**500 Internal Server Error**
```json
{
  "message": "An error occurred while processing your request."
}
```

### Error Message Types

- **Validation Errors** - Missing required fields or invalid data
- **Authentication Errors** - Invalid credentials or expired licenses
- **Authorization Errors** - Insufficient permissions
- **System Errors** - Database or server issues

## 🔧 Request Headers

### Required Headers

**Content-Type**
```
Content-Type: application/json
```

### Optional Headers

**User-Agent**
```
User-Agent: YourApp/1.0
```

**Accept**
```
Accept: application/json
```

## 📊 Response Formats

### Success Responses
All successful responses include:
- HTTP status code 200
- JSON response body
- Descriptive message
- Relevant data

### Error Responses
All error responses include:
- HTTP status code 4xx or 5xx
- JSON response body
- Error message
- No sensitive information

## 🔗 Related Documentation

- [Setup Guide](Setup-Guide)
- [Configuration Guide](Configuration)
- [Security Guide](Security-Guide)
- [Quick Start Guide](Quick-Start)
