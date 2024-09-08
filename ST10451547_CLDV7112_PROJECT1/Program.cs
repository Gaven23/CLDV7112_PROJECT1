using Azure.Data.Tables;
using ST10451547_CLDV7112_PROJECT1;
using ST10451547_CLDV7112_PROJECT1.BusinessLogic;
using ST10451547_CLDV7112_PROJECT1.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure TableServiceClient
builder.Services.AddSingleton<TableServiceClient>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    string connectionString = configuration.GetConnectionString("StorageConnectionString");
    return new TableServiceClient(connectionString);
});
builder.Services.AddTransient<IDataStore, CustomerProfileDataService>();
builder.Services.AddScoped<CustomerProfileService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
