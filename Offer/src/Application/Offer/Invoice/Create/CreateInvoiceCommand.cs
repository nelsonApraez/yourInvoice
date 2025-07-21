///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.Invoice.Create;

public record CreateInvoiceCommand(
    Guid Id,
    Guid OfferId,
    string Number,
    string ZipName,
    string Cufe,
    Guid StatusId,
    DateTime EmitDate,
    DateTime DueDate,
    decimal Total,
    decimal TotalAmount,
    Guid MoneyTypeId,
    decimal Trm,
    string ErrorMessage,
    DateTime NegotiationDate,
    decimal NegotiationTotal
    ) : IRequest<ErrorOr<Guid>>;