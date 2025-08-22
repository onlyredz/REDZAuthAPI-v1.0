# Configuration Guide - REDZAuth API

## 📋 Overview

This guide covers all configuration options available in the REDZAuth API, including database settings, security configurations, and webhook setup.

## 🔧 Configuration Files

### Main Configuration
- `appsettings.json` - Main configuration file
- `appsettings.Development.json` - Development-specific settings
- `appsettings.Example.json` - Example configuration template

## 🗄️ Database Configuration

### MongoDB Connection String

#### Local MongoDB
```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017"
  }
}
```

#### MongoDB Atlas (Cloud)
```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb+srv://username:password@cluster.mongodb.net/?retryWrites=true&w=majority"
  }
}
```

#### MongoDB with Authentication
```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://username:password@localhost:27017/database?authSource=admin"
  }
}
```

### Database Collections
The API automatically creates the following collections:
- **Users** - User accounts and authentication data
- **Licenses** - License keys and their usage status
- **Blacklist** - Banned users, IPs, and HWIDs

## 🔒 Security Configuration

### AES Encryption Key
```json
{
  "SecuritySettings": {
    "AesEncryptionKey": "YOUR_32_CHARACTER_ENCRYPTION_KEY_HERE"
  }
}
```

**Important Requirements:**
- Must be exactly 32 characters long
- Should contain a mix of letters, numbers, and special characters
- Keep this key secure and never commit it to version control

#### Generate a Secure Key
Use one of these methods to generate a 32-character key:

**PowerShell:**
```powershell
-join ((65..90) + (97..122) + (48..57) | Get-Random -Count 32 | % {[char]$_})
```

**Online Generator:**
- Use a secure password generator
- Ensure it generates exactly 32 characters

**Manual Generation:**
Create a key with a mix of:
- Uppercase letters (A-Z)
- Lowercase letters (a-z)
- Numbers (0-9)
- Special characters (!@#$%^&*)

## 🔔 Webhook Configuration

### Discord Webhook Setup

#### 1. Create Discord Webhook
1. Go to your Discord server
2. Right-click on a channel
3. Select "Edit Channel"
4. Go to "Integrations" tab
5. Click "Create Webhook"
6. Copy the webhook URL

#### 2. Configure in API
```json
{
  "WebhookSettings": {
    "DiscordWebhookUrl": "https://discord.com/api/webhooks/YOUR_WEBHOOK_URL"
  }
}
```

#### 3. Webhook Events
The API sends notifications for:
- **User Registration** - New account creation
- **Login Attempts** - Successful and failed logins
- **HWID Changes** - Device ID modifications
- **Ban/Unban Actions** - User management events
- **License Expiration** - Expired license attempts

### Disable Webhooks
To disable webhook notifications, either:
- Remove the `WebhookSettings` section
- Set the URL to an empty string
- Use the placeholder value: `"YOUR_DISCORD_WEBHOOK_URL_HERE"`

## 📊 Logging Configuration

### Log Levels
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

Available log levels:
- `Trace` - Most detailed logging
- `Debug` - Debug information
- `Information` - General information
- `Warning` - Warning messages
- `Error` - Error messages
- `Critical` - Critical errors

### Custom Logging
The API uses console logging by default. For production, consider:
- Implementing structured logging
- Using a logging framework like Serilog
- Sending logs to external services

## 🌐 Network Configuration

### Allowed Hosts
```json
{
  "AllowedHosts": "*"
}
```

For production, specify allowed hosts:
```json
{
  "AllowedHosts": "yourdomain.com,www.yourdomain.com"
}
```

### HTTPS Configuration
The API runs on HTTPS by default. Configure certificates in:
- `Properties/launchSettings.json` for development
- Server configuration for production

## 🔧 Environment-Specific Configuration

### Development Environment
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017"
  },
  "WebhookSettings": {
    "DiscordWebhookUrl": "YOUR_DISCORD_WEBHOOK_URL_HERE"
  },
  "SecuritySettings": {
    "AesEncryptionKey": "YOUR_32_CHARACTER_ENCRYPTION_KEY_HERE"
  }
}
```

### Production Environment
For production, use environment variables:

**Windows:**
```cmd
set ConnectionStrings__MongoDB="your-connection-string"
set SecuritySettings__AesEncryptionKey="your-encryption-key"
set WebhookSettings__DiscordWebhookUrl="your-webhook-url"
```

**Linux/macOS:**
```bash
export ConnectionStrings__MongoDB="your-connection-string"
export SecuritySettings__AesEncryptionKey="your-encryption-key"
export WebhookSettings__DiscordWebhookUrl="your-webhook-url"
```

**Docker:**
```dockerfile
ENV ConnectionStrings__MongoDB="your-connection-string"
ENV SecuritySettings__AesEncryptionKey="your-encryption-key"
ENV WebhookSettings__DiscordWebhookUrl="your-webhook-url"
```

## 🔄 License Plan Configuration

### Available Plans
The API supports these license plans:
- `mensal` - 30 days
- `trimestral` - 91 days
- `anual` - 366 days
- `lifetime` - 9999 days (effectively unlimited)

### Custom Plan Durations
To modify plan durations, edit the `GetDaysForPlan` method in `Services/LicenseService.cs`:

```csharp
private int GetDaysForPlan(string plan)
{
    return plan.ToLower() switch
    {
        "mensal" => 30,
        "trimestral" => 91,
        "anual" => 366,
        "lifetime" => 9999,
        "custom" => 60, // Add custom plans here
        _ => 30
    };
}
```

## 🔐 Advanced Security Configuration

### Password Hashing
The API uses SHA256 for password hashing. To change the algorithm, modify the `HashPassword` method in `Services/AuthService.cs`.

### HWID Validation
HWID validation is enabled by default. To disable or modify:
- Edit the login logic in `Services/AuthService.cs`
- Modify the HWID comparison logic

### IP Tracking
IP addresses are automatically captured from requests. To modify IP detection:
- Edit the IP extraction logic in controllers
- Consider using proxy headers for load balancers

## 📝 Configuration Validation

### Required Fields
The API validates these required configurations:
- MongoDB connection string
- AES encryption key (32 characters)
- Valid JSON format

### Validation Errors
If configuration is invalid, the API will:
- Log error messages to console
- Throw exceptions during startup
- Display clear error messages

## 🔗 Related Documentation

- [Setup Guide](Setup-Guide)
- [Security Guide](Security-Guide)
- [API Reference](API-Reference)
- [Quick Start Guide](Quick-Start)
