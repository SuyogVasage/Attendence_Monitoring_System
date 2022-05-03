

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Attendence_Monitoring_SystemContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnStr"));
});

builder.Services.AddScoped<IService<User, int>, UserService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();



//dotnet ef dbcontext scaffold "Data Source=SVASAGE-LAP-047\SQLEXPRESS;Initial Catalog=Attendence_Monitoring_System;Integrated Security=SSPI" Microsoft.EntityFrameworkCore.SqlServer -o Models
