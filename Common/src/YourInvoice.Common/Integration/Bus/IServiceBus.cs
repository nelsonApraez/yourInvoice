///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Common.Integration.Bus
{
    public interface IServiceBus
    {
        /// <summary>
        /// Inicialización del cliente del bus
        /// </summary>
        /// <param name="parameter"></param>
        void Start(ServiceBusParameters parameter);

        /// <summary>
        /// enviar un mensaje a una cola
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceBusMessage"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        Task SendMessageAsync<T>(T serviceBusMessage, string queueName);

        /// <summary>
        /// Enviar un mensaje a la cola colocando un tiempo futuro para ser procesada
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceBusMessage"></param>
        /// <param name="queueName"></param>
        /// <param name="scheduleDate"></param>
        /// <returns></returns>
        Task SendScheduleMessageAsync<T>(T serviceBusMessage, string queueName, DateTime scheduleDate);

        /// <summary>
        /// Enviar muchos mensajes a la cola en un solo proceso batch
        /// </summary>
        /// <param name="messagesQueue"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        Task SendMessageBatch(List<string> messagesQueue, string queueName);
    }
}