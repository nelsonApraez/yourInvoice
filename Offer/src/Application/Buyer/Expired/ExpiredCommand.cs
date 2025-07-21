///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Buyer.Expired;

public record ExpiredCommand() : IRequest<ErrorOr<bool>>;