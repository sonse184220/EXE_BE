using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using InnerChildApi;
using InnerChildApi.Common.Middleware;
using Repository;
using Repository.DataSeeder;
using Repository.SeedData;
using Service;

var builder = WebApplication.CreateBuilder(args);
if (FirebaseApp.DefaultInstance == null)
{
    var firebaseApp = FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromFile("firebase-adminsdk.json")
    });
}
builder.WebHost.UseUrls("http://0.0.0.0:5000");
builder.Services.AddControllers();

builder.Services.AddApplicationConfiguration(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddRepositories();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 104857600; // 100 MB
});

var app = builder.Build();
//data seeding
using (var scope = app.Services.CreateScope())
{
    await DataSeeder.SeedAllAsync(scope.ServiceProvider);
}
;

app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();


//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<SessionMiddleware>();
app.MapControllers();

app.Run();
