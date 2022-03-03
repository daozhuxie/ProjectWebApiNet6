using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var apiName = "测试Swagger-ProjectWebApiNet6";
var AllowSpecificOrigin = "AllowCors";

//builder.Services.AddSwaggerGen();// xxy
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = $"{apiName} 项目接口文档",//标题
        Version = "v1",//版本号
        Description = $"{apiName} 项目接口文档 - 版本：v1",//编辑描述
    });
    // 获取xml文件名
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // 获取xml文件路径
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // 添加控制器层注释，true表示显示控制器注释
    options.IncludeXmlComments(xmlPath, true);
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(AllowSpecificOrigin);// 增 允许所有跨域
    #region 添加Swagger有关中间件
    // 添加Swagger有关中间件
    //app.UseSwagger();//xxy
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((doc, item) =>
        {
            doc.Servers = new List<OpenApiServer>
                    {
                       new OpenApiServer(){ Url = $"{item.Scheme}://{item.Host.Value}/{item.Headers["X-Forwarded-Prefix"]}" }
                    };
        });
    });

    //配置SwaggerUI
    //app.UseSwaggerUI();//xxy
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = $"{apiName}档案项目接口文档";
        c.ShowExtensions();
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.RoutePrefix = "swagger";//如果根目录显示SwaggerUI加上这句: c.RoutePrefix = string.Empty();地址为：http://{ip}:{host}/{c.RoutePrefix}/index.html
    });
    #endregion
}

app.UseAuthorization();

app.MapControllers();

app.Run();
