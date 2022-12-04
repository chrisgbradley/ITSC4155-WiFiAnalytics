using Microsoft.EntityFrameworkCore;
using NinerFiVisualize.API.Data;
using NinerFiVisualize.API.Data.Services;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<TimedCacheRefreshService>();
builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("DistCache");
    options.SchemaName = "dbo";
    options.TableName = "TestCache";
});

builder.Services.AddMvc();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<NINERFIContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("NINERFI"),
    sqlServerOptions => sqlServerOptions.CommandTimeout(60)));
builder.Services.AddTransient<ChartsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.MapRazorPages();
    endpoints.MapControllerRoute("default", "api/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllers();
});

app.Run();
