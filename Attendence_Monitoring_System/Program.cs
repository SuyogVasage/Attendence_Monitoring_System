

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Attendence_Monitoring_SystemContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnStr"));
});

builder.Services.AddScoped<IService<User, int>, UserService>();
builder.Services.AddScoped<IService<UserLog, int>, UserLogService>();
builder.Services.AddScoped<IService<UserDetail, int>, UserDetailService>();
builder.Services.AddScoped<IService<AttendenceLog, int>, AttendenceLogService>();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});

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

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserLog}/{action=Create}/{id?}");

app.Run();



//dotnet ef dbcontext scaffold "Data Source=SVASAGE-LAP-047\SQLEXPRESS;Initial Catalog=Attendence_Monitoring_System;Integrated Security=SSPI" Microsoft.EntityFrameworkCore.SqlServer -o Models
