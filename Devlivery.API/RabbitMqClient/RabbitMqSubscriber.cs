using Devlivery.API.EventProcessor.Interface;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace Devlivery.API.RabbitMqClient
{                //,                Port = 
    public class RabbitMqSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private string _nomeDaFila;
        private IProcessaEvento _processaEvento;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqSubscriber(IConfiguration configuration, IProcessaEvento processaEvento)
        {
            _configuration = configuration;
            _connection = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMqHost"],
                Port = Int32.Parse(_configuration["RabbitMqPort"])
            }.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _nomeDaFila = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(
                queue: _nomeDaFila,
                exchange: "trigger",
                routingKey: ""
                );
            _processaEvento = processaEvento;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Subscrevendo ao RabbitMQ...");
            EventingBasicConsumer? consumidor = new EventingBasicConsumer(_channel);

            consumidor.Received += (ModuleHandle, ea) =>
            { 
               ReadOnlyMemory<byte> body = ea.Body;
               string? mensagem = Encoding.UTF8.GetString(body.ToArray());
                //Processa a mensagem
                _processaEvento.Processa(mensagem);

            };

            _channel.BasicConsume(queue: _nomeDaFila, autoAck: true, consumer:consumidor);


            return Task.CompletedTask;
        }
      
    }
}