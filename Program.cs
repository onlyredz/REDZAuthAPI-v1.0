using MongoDB.Driver;
using REDZAuthApi.Services;
using REDZAuthApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure MongoDB
builder.Services.AddSingleton<IMongoClient>(s =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB") 
        ?? throw new InvalidOperationException("MongoDB connection string not found");
    return new MongoClient(connectionString);
});

builder.Services.AddScoped(s =>
    s.GetRequiredService<IMongoClient>().GetDatabase("REDZAuth"));

// Register application services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<BlacklistService>();
builder.Services.AddScoped<LicenseService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MongoDbService>();

// Add HTTP client for webhooks
builder.Services.AddHttpClient();

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initialize AesHelper with configuration
var encryptionKey = builder.Configuration["SecuritySettings:AesEncryptionKey"];
if (!string.IsNullOrEmpty(encryptionKey) && encryptionKey != "YOUR_32_CHARACTER_ENCRYPTION_KEY_HERE")
{
    AesHelper.Initialize(encryptionKey);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
