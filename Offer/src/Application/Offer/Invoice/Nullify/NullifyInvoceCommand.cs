///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.Invoice.Nullify;

public record NullifyInvoceCommand(Guid offerId) : IRequest<ErrorOr<bool>>;