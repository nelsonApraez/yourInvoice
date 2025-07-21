///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.Invoice.DeleteByIds;

public record DeleteOfferInvoiceByIdsCommand(List<Guid> invoiceIds) : IRequest<ErrorOr<bool>>;