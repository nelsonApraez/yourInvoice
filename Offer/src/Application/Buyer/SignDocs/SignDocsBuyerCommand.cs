///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Offer.SignDocs;

namespace yourInvoice.Offer.Application.Buyer.SignDocs;
public record class SignDocsBuyerCommand(int numberOffer) : IRequest<ErrorOr<SignDocsResponse>>;