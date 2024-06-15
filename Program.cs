using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SampleWebApp.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Load configuration from Azure Key Vault
builder.Configuration.AddAzureKeyVault(
    new Uri(builder.Configuration["AzureKeyVault:Vault"]),
    new DefaultAzureCredential()
);

builder.Services.AddRazorPages();
builder.Services.AddSingleton<DAL>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
