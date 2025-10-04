// 应用程序入口点
/**
主要作用：配置依赖注入DI，加载配置文件appsettings.json,注册中间件middleware和http管道,启动ASP.NET Core Host， 类比于springboot中的application.java
*/
//1. 创建webApplicationBuilder(构建器阶段)：负责加载配置、注入依赖、注册服务
//加载配置文件: appsetting.json/appsetting.development.json
//创建依赖注入容器
//注册默认服务,日志，环境，配置
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//向DI container中注册openAPI/Swagger服务，自动生成接口文档
builder.Services.AddOpenApi();
//给予前面配置，构建最终的WebApplication对象，准备运行时http server
var app = builder.Build();

// Configure the HTTP request pipeline.判断当前运行环境（是否为开发模式）
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
