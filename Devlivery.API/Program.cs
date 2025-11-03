using Devlivery.API.Data;
using Devlivery.API.Data.Interface;
using Devlivery.API.EventProcessor;
using Devlivery.API.EventProcessor.Interface;
using Devlivery.API.Profiles;
using Devlivery.API.RabbitMqClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DevliveryConnection");


// Add services to the container.
builder.Services.AddDbContext<VagaContext>(opts =>
    opts.UseSqlServer(connectionString));

//builder.Services.AddDbContext<VagaContext>(opts =>
//    opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddAutoMapper(typeof(VagaProfile).Assembly);
builder.Services.AddControllers().AddNewtonsoftJson();
//builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddScoped<IVagaRepository, VagaRepository>();
builder.Services.AddHostedService<RabbitMqSubscriber>();
builder.Services.AddSingleton<IProcessaEvento, ProcessaEvento>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FilmesAPI", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
//builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<VagaContext>();

        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: "+ex.ToString());
    }
}
app.Run();
