///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Azure.Core;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using System.Text.Json;

namespace yourInvoice.Common.Integration.Bus
{
    public class ServiceBus : IServiceBus, IDisposable
    {
        private ServiceBusClient _client;
        private readonly string errorClientInitMessage = "El cliente del service bus no ha sido inicializado";

        public void Start(ServiceBusParameters parameter)
        {
            if (parameter.AuthenticationAD)
            {
                TokenCredential credential = new ClientSecretCredential(parameter.TenantId, parameter.ClientId, parameter.ClientSecret);
                var clientOptions = new ServiceBusClientOptions
                {
                    TransportType = ServiceBusTransportType.AmqpWebSockets
                };
                _client = new ServiceBusClient(
                    parameter.FullyQualifiedNamespace,
                    credential,
                    clientOptions);
            }
            else
                _client = new ServiceBusClient(parameter.ConnectionString);
        }

        public async Task SendMessageAsync<T>(T serviceBusMessage, string queueName)
        {
            if (_client == null)
                throw new ArgumentException(errorClientInitMessage);

            ServiceBusSender sender = _client.CreateSender(queueName);
            var message = ConvertMessage(serviceBusMessage);
            await sender.SendMessageAsync(message);
        }

        public async Task SendScheduleMessageAsync<T>(T serviceBusMessage, string queueName, DateTime scheduleDate)
        {
            if (_client == null)
                throw new ArgumentException(errorClientInitMessage);

            ServiceBusSender sender = _client.CreateSender(queueName);
            var message = ConvertMessage(serviceBusMessage);
            await sender.ScheduleMessageAsync(message, scheduleDate);
        }

        public async Task SendMessageBatch(List<string> messagesQueue, string queueName)
        {
            if (_client == null)
                throw new ArgumentException(errorClientInitMessage);

            ServiceBusSender sender = _client.CreateSender(queueName);
            Queue<ServiceBusMessage> messages = new Queue<ServiceBusMessage>();
            foreach (var message in messagesQueue)
                messages.Enqueue(new ServiceBusMessage(message));

            while (messages.Count > 0)
            {
                using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

                if (messageBatch.TryAddMessage(messages.Peek()))
                {
                    messages.Dequeue();
                }

                while (messages.Count > 0 && messageBatch.TryAddMessage(messages.Peek()))
                {
                    messages.Dequeue();
                }
                await sender.SendMessagesAsync(messageBatch);
            }
        }

        private static ServiceBusMessage ConvertMessage<T>(T serviceBusMessage)
        {
            string messageBody = serviceBusMessage.ToString();
            if (serviceBusMessage.GetType().Name != nameof(String))
            {
                messageBody = JsonSerializer.Serialize(serviceBusMessage);
            }
            return new ServiceBusMessage(messageBody);
        }

        private bool disposedValue;

        ~ServiceBus()
           => Dispose(disposing: false);

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && _client is not null)
                {
                    _client.DisposeAsync();
                }
                disposedValue = true;
            }
        }
    }
}