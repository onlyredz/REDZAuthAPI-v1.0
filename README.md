# REDZAuth API

A comprehensive license key authentication system built with ASP.NET Core 8.0 and MongoDB. This API provides secure user authentication, license key management, HWID binding, and blacklist functionality with Discord webhook integration for monitoring.

## 🎯 Purpose

This API was created for personal use and study.

It was designed to serve as authentication/logs/registrations for my C# projects. In real tests, it wasn't used much because I didn't have much experience or time to dedicate to it. However, during interaction testing using POSTMAN, it performed very well and was functional. I just needed to implement it in my productions (which I didn't do, by the way, lol). But it's a good API, well-structured, and designed to replace KeyAuth.cs and offer a higher level of security, AES 256. Well, now it's open source, enjoy it.

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK
- MongoDB instance (local or cloud)

### Setup
```bash
git clone https://github.com/onlyredz/REDZAuthAPI-v1.0.git
cd REDZAuthAPI-v1.0
cp appsettings.Example.json appsettings.json
# Edit appsettings.json with your configuration
dotnet restore && dotnet run
```

Access the API documentation at: `https://localhost:7001/swagger`

## 🛠️ Technology Stack

- **.NET 8.0** - Latest LTS version
- **ASP.NET Core Web API** - Modern web framework
- **MongoDB** - NoSQL database for flexible data storage
- **MongoDB.Driver** - Official MongoDB driver for .NET
- **Swagger/OpenAPI** - API documentation and testing
- **Dependency Injection** - Built-in .NET DI container

## 🔧 Frameworks & Libraries Used

- **Microsoft.AspNetCore.Authentication** (v2.3.0) - Authentication framework
- **Microsoft.Extensions.Configuration.UserSecrets** (v8.0.0) - Configuration management
- **MongoDB.Driver** (v2.24.0) - MongoDB driver for .NET
- **Swashbuckle.AspNetCore** (v6.5.0) - API documentation

## ⚠️ Security Notice

**IMPORTANT**: This codebase has been prepared for open-source release, but please be aware of the following security considerations:

- **No Authentication/Authorization**: The admin endpoints currently have no authentication. In production, implement proper authentication (JWT, API keys, etc.).
- **No Rate Limiting**: No rate limiting is implemented. Consider adding rate limiting for production use.
- **No CORS**: CORS policies are not configured. Configure appropriate CORS policies for your domain.
- **HTTPS**: Ensure HTTPS is properly configured in production.
- **Input Validation**: While basic validation exists, consider implementing more robust input validation.
- **Logging**: Current logging uses Console.WriteLine. Implement proper structured logging for production.

**Always review and test security measures before deploying to production!**

## 📚 Documentation

For detailed documentation, setup instructions, and API reference, please visit the **[Project Wiki](https://github.com/onlyredz/REDZAuthAPI-v1.0/wiki)**.

### Available Documentation:
- 📖 [Complete Setup Guide](https://github.com/onlyredz/REDZAuthAPI-v1.0/wiki/Setup-Guide)
- 🔧 [Configuration Guide](https://github.com/onlyredz/REDZAuthAPI-v1.0/wiki/Configuration)
- 🔌 [API Reference](https://github.com/onlyredz/REDZAuthAPI-v1.0/wiki/API-Reference)
- 🔒 [Security Guide](https://github.com/onlyredz/REDZAuthAPI-v1.0/wiki/Security-Guide)
- 🚀 [Quick Start Guide](https://github.com/onlyredz/REDZAuthAPI-v1.0/wiki/Quick-Start)
- 🇧🇷 [Documentação em Português](https://github.com/onlyredz/REDZAuthAPI-v1.0/wiki/PT‐BR-FULL-GUIDE)

## 🔌 Main Features

- **User Authentication** (Register/Login with HWID binding)
- **License Key Management** (Generate, validate, track usage)
- **Blacklist System** (Ban/unban users, IPs, HWIDs)
- **Admin Functions** (HWID reset, key management)
- **Discord Webhook Integration** for monitoring
- **Status Page** for API health monitoring

## 👨‍💻 Credits

**Original Developer**: [onlyredz](https://github.com/onlyredz)

**Security Audit & Open Source Preparation**: This codebase has been prepared for open-source release with security improvements and comprehensive documentation.

**Please maintain the original credits when using or modifying this code.**

## 📄 License

This project is licensed under the Apashe 2.0 License - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📞 Support

For support and questions:
- Create an issue in the GitHub repository
- Check the API documentation at `/swagger`
- Visit the [Project Wiki](https://github.com/onlyredz/REDZAuthAPI-v1.0/wiki)

---

**Note**: This API is designed for educational and legitimate business use. Please ensure compliance with applicable laws and regulations in your jurisdiction. Always review and test security measures before deploying to production.
