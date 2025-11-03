using Usuarios.API.Aplicacao;
using Usuarios.API.Aplicacao.Interface;
using Usuarios.API.Infraestrutura;
using Usuarios.API.Infraestrutura.Interface;

using Dapper;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddSingleton<IRabbitMqClient, RabbitMqClient>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient<IProfissionalHttpService, ProfissionalHttpService>();

builder.Services.AddSwaggerGen();


static void InitializeDatabase(IHost app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var config = services.GetRequiredService<IConfiguration>();

    logger.LogInformation("Iniciando a inicialização do banco de dados...");

    try
    {
        var connectionString = config.GetConnectionString("UsuarioConnection");
        var masterConnectionString = new SqlConnectionStringBuilder(connectionString)
        {
            InitialCatalog = "master" 
        }.ConnectionString;

        var scriptPath = "init.sql"; 
        var maxRetries = 15;
        var retryDelay = TimeSpan.FromSeconds(5);
        var retries = 0;

        while (retries < maxRetries)
        {
            try
            {
                logger.LogInformation("Tentando conectar ao SQL Server (Tentativa {count})...", retries + 1);

                var sqlScript = File.ReadAllText(scriptPath);

                var batches = Regex.Split(sqlScript, @"^\s*GO\s*$",
                                          RegexOptions.Multiline | RegexOptions.IgnoreCase);


                using (var connection = new SqlConnection(masterConnectionString))
                {
                    connection.Open();

                    foreach (var batch in batches)
                    {
                        if (string.IsNullOrWhiteSpace(batch)) continue;

                        logger.LogInformation("Executando lote SQL...");
                        connection.Execute(batch);
                    }
                }

                logger.LogInformation("Script de inicialização do banco executado com sucesso.");
                break;
            }
            catch (Exception ex)
            {
                retries++;
                logger.LogWarning(ex, "Falha ao conectar/executar script (Tentativa {count}). Aguardando {delay}s...", retries, retryDelay.Seconds);
                Task.Delay(retryDelay).Wait(); // Espera 5 segundos
            }
        }

        if (retries == maxRetries)
        {
            logger.LogCritical("Não foi possível inicializar o banco de dados após {maxRetries} tentativas.", maxRetries);
            throw new Exception("Falha na inicialização do banco de dados.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Um erro ocorreu ao inicializar o banco de dados.");
        throw; 
    }
}


var app = builder.Build();

InitializeDatabase(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
