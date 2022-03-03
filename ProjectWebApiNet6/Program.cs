using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var apiName = "����Swagger-ProjectWebApiNet6";
var AllowSpecificOrigin = "AllowCors";

//builder.Services.AddSwaggerGen();// xxy
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = $"{apiName} ��Ŀ�ӿ��ĵ�",//����
        Version = "v1",//�汾��
        Description = $"{apiName} ��Ŀ�ӿ��ĵ� - �汾��v1",//�༭����
    });
    // ��ȡxml�ļ���
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // ��ȡxml�ļ�·��
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // ��ӿ�������ע�ͣ�true��ʾ��ʾ������ע��
    options.IncludeXmlComments(xmlPath, true);
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(AllowSpecificOrigin);// �� �������п���
    #region ���Swagger�й��м��
    // ���Swagger�й��м��
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

    //����SwaggerUI
    //app.UseSwaggerUI();//xxy
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = $"{apiName}������Ŀ�ӿ��ĵ�";
        c.ShowExtensions();
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.RoutePrefix = "swagger";//�����Ŀ¼��ʾSwaggerUI�������: c.RoutePrefix = string.Empty();��ַΪ��http://{ip}:{host}/{c.RoutePrefix}/index.html
    });
    #endregion
}

app.UseAuthorization();

app.MapControllers();

app.Run();
