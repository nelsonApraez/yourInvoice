///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Buyer.Reject;

public record RejectCommand(int numberOffer) : IRequest<ErrorOr<bool>>;