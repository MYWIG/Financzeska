
using FinanczeskaServerApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting;


var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<ChatService>();
builder.Services.AddTransient<JsonDataSerializer>();
builder.Services.AddTransient<ModelCallerService>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders(); // Remove other logging providers
    loggingBuilder.SetMinimumLevel(LogLevel.Trace); // Adjust log level as needed

});

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
