using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Shop.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


builder.Services.AddHttpClient<IUserPanelService, UserPanelService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5265/"); // Api'nin calistigi url (api bazli proje oldugu icin)
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
