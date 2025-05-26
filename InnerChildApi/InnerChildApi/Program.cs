using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using InnerChildApi;
using InnerChildApi.Common.Hubs;
using InnerChildApi.Common.Middleware;
using Repository;
using Repository.DataSeeder;
using Service;
using Service.BackgroundJobs;

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
app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<PurchaseCheckJob>(
    "check-purchase-expiry",
    job => job.Run(),
    Cron.Daily);
RecurringJob.AddOrUpdate<AuthSessionJob>(
    "remove-inactive-session",
    job => job.Run(),
    Cron.Weekly);
RecurringJob.AddOrUpdate<CleanTokenJob>(
    "remove-revoked-token",
    job => job.Run(),
    Cron.Weekly);
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<SessionMiddleware>();
app.MapControllers();
app.MapHub<SessionHub>("/hub/session");
app.Run();
