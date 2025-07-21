///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using MediatR;

namespace yourInvoice.Common.Primitives
{
    public record DomainEvent(Guid Id) : INotification;
}