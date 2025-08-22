# 🏠 Welcome to REDZAuth API

<div align="center">

# 🔐 REDZAuth API

**A comprehensive license key authentication system built with ASP.NET Core 8.0 and MongoDB**

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=.net)](https://dotnet.microsoft.com/)
[![MongoDB](https://img.shields.io/badge/MongoDB-4.4+-47A248?style=for-the-badge&logo=mongodb)](https://www.mongodb.com/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](LICENSE)
[![GitHub](https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white)](https://github.com/onlyredz)

</div>

---

## 🎯 What is REDZAuth API?

REDZAuth API is a powerful and secure authentication system designed for software licensing and user management. It provides a complete solution for:

- **🔑 License Key Management** - Generate, validate, and track license keys
- **👤 User Authentication** - Secure user registration and login with HWID binding
- **🛡️ Security Features** - HWID binding, IP tracking, and blacklist system
- **📊 Monitoring** - Real-time Discord webhook notifications
- **⚡ Performance** - Built with modern .NET 8.0 and MongoDB

## 🚀 Quick Start

Get up and running in **5 minutes**:

```bash
# 1. Clone the repository
git clone https://github.com/onlyredz/REDZAuthAPI-v1.0.git
cd REDZAuthAPI-v1.0

# 2. Configure settings
cp appsettings.Example.json appsettings.json
# Edit appsettings.json with your configuration

# 3. Run the application
dotnet restore && dotnet run

# 4. Access the API
# Swagger UI: https://localhost:7001/swagger
# Status Page: https://localhost:7001/apistatus
```

## 📋 System Architecture

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Client App    │    │  REDZAuth API   │    │    MongoDB      │
│                 │    │                 │    │                 │
│ • License Check │◄──►│ • Authentication│◄──►│ • Users         │
│ • HWID Binding  │    │ • Key Management│    │ • Licenses      │
│ • User Login    │    │ • Blacklist     │    │ • Blacklist     │
└─────────────────┘    └─────────────────┘    └─────────────────┘
                              │
                              ▼
                       ┌─────────────────┐
                       │  Discord Webhook│
                       │                 │
                       │ • Notifications │
                       │ • Monitoring    │
                       └─────────────────┘
```

## 🔧 Key Features

### 🔐 Authentication System
- **User Registration** with license key validation
- **Secure Login** with HWID (Hardware ID) binding
- **Password Hashing** using SHA256
- **IP Address Tracking** for security monitoring

### 🔑 License Management
- **Automatic Key Generation** with customizable formats
- **Multiple Plan Types**: Monthly, Quarterly, Annual, Lifetime
- **License Validation** and usage tracking
- **Custom License Keys** for specific use cases

### 🛡️ Security Features
- **HWID Binding** for device-specific access control
- **Blacklist System** for banning users, IPs, and HWIDs
- **Discord Webhook Integration** for real-time monitoring
- **HWID Mismatch Detection** to prevent unauthorized access

### 👨‍💼 Admin Functions
- **HWID Reset** functionality for legitimate device changes
- **License Key Management** (generate, list, filter)
- **User Ban/Unban** operations
- **Status Page** for API health monitoring

## 📊 API Endpoints Overview

| Category | Endpoint | Method | Description |
|----------|----------|--------|-------------|
| **Authentication** | `/api/auth/register` | POST | Register new user |
| | `/api/auth/login` | POST | User login |
| **License Management** | `/api/license/generate` | POST | Generate license key |
| | `/api/license/custom` | POST | Create custom license |
| | `/api/key/list` | GET | List license keys |
| **Admin Functions** | `/api/admin/reset-hwid` | POST | Reset user HWID |
| | `/api/ban` | POST | Ban user/IP/HWID |
| | `/api/unban` | POST | Unban user/IP/HWID |
| **Status** | `/apistatus` | GET | API health status |

## 🛠️ Technology Stack

- **.NET 8.0** - Latest LTS version with modern features
- **ASP.NET Core Web API** - High-performance web framework
- **MongoDB** - Flexible NoSQL database
- **MongoDB.Driver** - Official .NET driver
- **Swagger/OpenAPI** - Interactive API documentation
- **Dependency Injection** - Built-in .NET DI container

## 📚 Documentation

### 📖 Getting Started
- **[Setup Guide](Setup-Guide)** - Detailed installation instructions
- **[Quick Start Guide](Quick-Start)** - 5-minute setup guide
- **[Configuration Guide](Configuration)** - All configuration options

### 🔌 API Reference
- **[Complete API Reference](API-Reference)** - All endpoints with examples
- **[Security Guide](Security-Guide)** - Production security considerations

### 🌍 International
- **[Documentação em Português](Documentação-em-Português)** - Complete Portuguese documentation

## ⚠️ Security Notice

**IMPORTANT**: This codebase has been prepared for open-source release, but please be aware of the following security considerations:

- **No Authentication/Authorization**: The admin endpoints currently have no authentication
- **No Rate Limiting**: No rate limiting is implemented
- **No CORS**: CORS policies are not configured
- **HTTPS**: Ensure HTTPS is properly configured in production
- **Input Validation**: While basic validation exists, consider implementing more robust validation
- **Logging**: Current logging uses Console.WriteLine

**Always review and test security measures before deploying to production!**

## 🎯 Use Cases

### Software Licensing
- Generate license keys for different subscription plans
- Validate licenses on application startup
- Track license usage and expiration
- Handle license renewals and upgrades

### User Management
- Secure user registration and authentication
- Device binding with HWID for security
- IP tracking for suspicious activity detection
- Comprehensive user ban/unban system

### Monitoring & Analytics
- Real-time Discord notifications for security events
- License usage analytics
- User activity tracking
- System health monitoring

## 🔄 Workflow Examples

### User Registration Flow
```
1. Generate License Key (Admin)
   └── POST /api/license/generate?plan=mensal
       └── Returns: MENSAL-ABCD-1234

2. User Registration
   └── POST /api/auth/register
       └── Validates license key
       └── Creates user account
       └── Binds HWID to account

3. User Login
   └── POST /api/auth/login
       └── Validates credentials
       └── Checks HWID binding
       └── Returns user info
```

### License Management Flow
```
1. Create License Keys
   └── POST /api/license/generate?plan=anual
   └── POST /api/license/custom?customKey=PREMIUM-2024&plan=lifetime

2. Monitor Usage
   └── GET /api/key/list?plan=mensal&used=true
   └── Check expiration dates
   └── Track usage patterns

3. Handle Violations
   └── POST /api/ban (ban user/IP/HWID)
   └── POST /api/unban (remove from blacklist)
```

## 🚀 Performance & Scalability

- **High Performance**: Built with ASP.NET Core 8.0 for optimal performance
- **Scalable**: MongoDB provides horizontal scaling capabilities
- **Reliable**: Comprehensive error handling and logging
- **Flexible**: Easy to customize and extend for specific needs

## 🤝 Contributing

We welcome contributions! Please see our contributing guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Credits

**Original Developer**: [onlyredz](https://github.com/onlyredz)

**Security Audit & Open Source Preparation**: This codebase has been prepared for open-source release with security improvements and comprehensive documentation.

**Please maintain the original credits when using or modifying this code.**

## 📞 Support

For support and questions:

- 📖 **Documentation**: Check the guides above
- 🐛 **Issues**: Create an issue in the GitHub repository
- 🔧 **API Testing**: Use the Swagger UI at `/swagger`
- 📊 **Status**: Check the status page at `/apistatus`

## 🔗 Quick Links

- **[Setup Guide](Setup-Guide)** - Get started quickly
- **[API Reference](API-Reference)** - Complete endpoint documentation
- **[Security Guide](Security-Guide)** - Production security checklist
- **[Configuration Guide](Configuration)** - All configuration options
- **[Quick Start](Quick-Start)** - 5-minute setup
- **[Portuguese Documentation](Documentação-em-Português)** - Documentação em Português

---

<div align="center">

**Made with ❤️ by [onlyredz](https://github.com/onlyredz)**

[![GitHub stars](https://img.shields.io/github/stars/onlyredz/REDZAuthAPI-v1.0?style=social)](https://github.com/onlyredz/REDZAuthAPI-v1.0)
[![GitHub forks](https://img.shields.io/github/forks/onlyredz/REDZAuthAPI-v1.0?style=social)](https://github.com/onlyredz/REDZAuthAPI-v1.0)
[![GitHub issues](https://img.shields.io/github/issues/onlyredz/REDZAuthAPI-v1.0)](https://github.com/onlyredz/REDZAuthAPI-v1.0/issues)

</div>
