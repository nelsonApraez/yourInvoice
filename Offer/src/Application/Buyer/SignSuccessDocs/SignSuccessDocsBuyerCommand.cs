///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Buyer.SignSuccessDocs;
public record class SignSuccessDocsBuyerCommand(int numberOffer) : IRequest<ErrorOr<bool>>;