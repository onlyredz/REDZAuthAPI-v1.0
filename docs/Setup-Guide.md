# Setup Guide - REDZAuth API

## 📋 Prerequisites

Before running this application, ensure you have:

- **.NET 8.0 SDK** installed on your machine
- **MongoDB** instance running (local or cloud)
- **Visual Studio 2022** or **VS Code** (recommended)
- **Git** for version control

## 🔧 Installation & Setup

### Option 1: Visual Studio 2022

#### 1. Clone the Repository
```bash
git clone https://github.com/onlyredz/REDZAuthAPI-v1.0.git
cd REDZAuthAPI-v1.0
```

#### 2. Open in Visual Studio
- Open Visual Studio 2022
- Go to `File` → `Open` → `Project/Solution`
- Navigate to the cloned folder and select `REDZAuthApi.sln`
- Click `Open`

#### 3. Configure Database Connection
- In Solution Explorer, right-click on `appsettings.json`
- Select `Open` or double-click to open
- Update the MongoDB connection string:

```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017"
  }
}
```

For MongoDB Atlas (cloud), use:
```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb+srv://username:password@cluster.mongodb.net/?retryWrites=true&w=majority"
  }
}
```

#### 4. Configure Security Settings
In the same `appsettings.json` file, update the security settings:

```json
{
  "SecuritySettings": {
    "AesEncryptionKey": "YOUR_32_CHARACTER_ENCRYPTION_KEY_HERE"
  }
}
```

**Important**: Replace `YOUR_32_CHARACTER_ENCRYPTION_KEY_HERE` with a secure 32-character encryption key.

#### 5. Configure Discord Webhook (Optional)
To enable Discord notifications, add your webhook URL:

```json
{
  "WebhookSettings": {
    "DiscordWebhookUrl": "https://discord.com/api/webhooks/YOUR_WEBHOOK_URL"
  }
}
```

#### 6. Build and Run
- Press `Ctrl + Shift + B` to build the solution
- Press `F5` to run the application in debug mode
- Or press `Ctrl + F5` to run without debugging

The API will be available at `https://localhost:7001` (or the port shown in the console).

### Option 2: Visual Studio Code

#### 1. Clone the Repository
```bash
git clone https://github.com/onlyredz/REDZAuthAPI-v1.0.git
cd REDZAuthAPI-v1.0
```

#### 2. Open in VS Code
```bash
code .
```

#### 3. Install Required Extensions
Install the following VS Code extensions:
- **C#** (by Microsoft)
- **C# Dev Kit** (by Microsoft)
- **REST Client** (optional, for testing API endpoints)

#### 4. Configure Database Connection
- Open `appsettings.json` in VS Code
- Update the MongoDB connection string as shown in the Visual Studio section above

#### 5. Configure Security Settings
Update the security settings in `appsettings.json` as shown in the Visual Studio section above.

#### 6. Configure Discord Webhook (Optional)
Add your Discord webhook URL as shown in the Visual Studio section above.

#### 7. Build and Run
Open the integrated terminal in VS Code (`Ctrl + `` `) and run:

```bash
dotnet restore
dotnet build
dotnet run
```

The API will be available at `https://localhost:7001` (or the port shown in the console).

### Option 3: Command Line Only

#### 1. Clone and Setup
```bash
git clone https://github.com/onlyredz/REDZAuthAPI-v1.0.git
cd REDZAuthAPI-v1.0
```

#### 2. Configure Settings
- Copy `appsettings.Example.json` to `appsettings.json`
- Edit `appsettings.json` with your configuration as shown above

#### 3. Run the Application
```bash
dotnet restore
dotnet build
dotnet run
```

## 🐛 Troubleshooting

### Common Issues

1. **MongoDB Connection Failed**
   - Verify your connection string in `appsettings.json`
   - Ensure MongoDB is running and accessible
   - Check network connectivity for cloud instances

2. **Encryption Key Error**
   - Ensure your AES encryption key is exactly 32 characters
   - Update the key in `appsettings.json`

3. **Webhook Not Working**
   - Verify your Discord webhook URL is correct
   - Check Discord server permissions
   - Review console logs for webhook errors

4. **HWID Mismatch**
   - This is expected behavior for security
   - Use admin endpoint to reset HWID if legitimate

5. **Build Errors**
   - Ensure .NET 8.0 SDK is installed
   - Run `dotnet restore` to restore packages
   - Check for any missing dependencies

### Visual Studio Specific Issues

1. **Solution Won't Load**
   - Ensure Visual Studio 2022 is updated
   - Install .NET 8.0 workload if prompted
   - Try cleaning and rebuilding the solution

2. **Debugging Issues**
   - Check that the startup project is set correctly
   - Verify launch settings in `Properties/launchSettings.json`

### VS Code Specific Issues

1. **IntelliSense Not Working**
   - Install the C# extension
   - Reload VS Code after installation
   - Run `dotnet restore` in the terminal

2. **Debugging Not Working**
   - Install the C# Dev Kit extension
   - Create a launch configuration if needed

## 📚 Next Steps

After successful setup:

1. **Test the API** using the Swagger UI at `/swagger`
2. **Read the API Reference** for endpoint documentation
3. **Configure your Discord webhook** for monitoring
4. **Review the Security Guide** for production considerations
5. **Check the Configuration Guide** for advanced settings

## 🔗 Related Documentation

- [Configuration Guide](Configuration)
- [API Reference](API-Reference)
- [Security Guide](Security-Guide)
- [Quick Start Guide](Quick-Start)
