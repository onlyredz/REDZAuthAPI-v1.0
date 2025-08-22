# Quick Start Guide - REDZAuth API

## 🚀 Get Up and Running in 5 Minutes

### Prerequisites
- .NET 8.0 SDK installed
- MongoDB instance running (local or cloud)

### Step 1: Clone and Setup
```bash
git clone https://github.com/onlyredz/REDZAuthAPI-v1.0.git
cd REDZAuthAPI-v1.0
```

### Step 2: Configure Settings
Copy the example configuration:
```bash
cp appsettings.Example.json appsettings.json
```

Edit `appsettings.json` with your settings:
```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017"
  },
  "SecuritySettings": {
    "AesEncryptionKey": "MySuperSecretKey32CharactersLong123"
  },
  "WebhookSettings": {
    "DiscordWebhookUrl": "https://discord.com/api/webhooks/YOUR_WEBHOOK_URL"
  }
}
```

### Step 3: Run the Application
```bash
dotnet restore
dotnet run
```

### Step 4: Test the API
1. Open your browser and go to: `https://localhost:7001/swagger`
2. Test the endpoints using the interactive Swagger UI

## 🔧 Common Quick Configurations

### Local MongoDB
```json
"ConnectionStrings": {
  "MongoDB": "mongodb://localhost:27017"
}
```

### MongoDB Atlas (Cloud)
```json
"ConnectionStrings": {
  "MongoDB": "mongodb+srv://username:password@cluster.mongodb.net/?retryWrites=true&w=majority"
}
```

### Generate a 32-Character Key
Use this PowerShell command to generate a secure key:
```powershell
-join ((65..90) + (97..122) + (48..57) | Get-Random -Count 32 | % {[char]$_})
```

## 🧪 Quick API Tests

### 1. Generate a License Key
```bash
curl -X POST "https://localhost:7001/api/license/generate?plan=mensal"
```

### 2. Register a User
```bash
curl -X POST "https://localhost:7001/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "password123",
    "key": "MENSAL-ABCD-1234",
    "hwid": "test-hwid-123",
    "ip": "192.168.1.1"
  }'
```

### 3. Login User
```bash
curl -X POST "https://localhost:7001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "password123",
    "hwid": "test-hwid-123",
    "ip": "192.168.1.1"
  }'
```

### 4. Check API Status
```bash
curl -X GET "https://localhost:7001/apistatus"
```

## 🐛 Quick Troubleshooting

### Port Already in Use
If you get a port conflict, the API will automatically use the next available port. Check the console output for the correct URL.

### MongoDB Connection Failed
1. Ensure MongoDB is running
2. Check your connection string
3. For local MongoDB: `mongodb://localhost:27017`
4. For cloud: Use your MongoDB Atlas connection string

### Build Errors
```bash
dotnet clean
dotnet restore
dotnet build
```

## 📚 Next Steps

1. **Read the full Setup Guide** for detailed instructions
2. **Check the Configuration Guide** for advanced settings
3. **Review the Security Guide** for production considerations
4. **Test all endpoints** using the Swagger UI
5. **Configure your Discord webhook** for monitoring

## ⚠️ Important Notes

- This is a **development setup** - add authentication for production
- The admin endpoints are **unprotected** - implement proper security
- Use **HTTPS** in production environments
- **Backup your database** regularly
- **Monitor logs** for suspicious activity

## 🔗 Related Documentation

- [Setup Guide](Setup-Guide)
- [Configuration Guide](Configuration)
- [API Reference](API-Reference)
- [Security Guide](Security-Guide)

---

**Need help?** Check the main README.md or create an issue in the GitHub repository.
