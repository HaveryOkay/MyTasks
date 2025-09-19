using Microsoft.EntityFrameworkCore;
using MyTasks.Core.Interfaces;
using MyTasks.Infrastructure;
using MyTasks.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ✅ 注册 DbContext（使用 SQLite）
var connectionString = builder.Configuration.GetConnectionString("Sqlite") ?? "Data Source=tasks.db";
builder.Services.AddDbContext<TasksDbContext>(options =>
    options.UseSqlite(connectionString));

// 注册 Repository（Scoped）
builder.Services.AddScoped<ITaskRepository, EfTaskRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();