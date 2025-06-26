using MongoDB.Driver;
using REDZAuthApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient("mongodb+srv://onlyredzdev:onlyredzdev@redzauth.qt32rob.mongodb.net/?retryWrites=true&w=majority"));

builder.Services.AddScoped(s =>
    s.GetRequiredService<IMongoClient>().GetDatabase("REDZAuth"));

builder.Services.AddScoped<AuthService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
