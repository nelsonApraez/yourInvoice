///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Azure.Messaging.ServiceBus;

using Microsoft.Azure.Functions.Worker;

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.DIAN.Function.Business
{
    public interface IIntegrationDianBusiness
    {
        Task<bool> ValidationProcessDIAN(ServiceBusReceivedMessage message, ServiceBusMessageActions messageAction);
    }
}