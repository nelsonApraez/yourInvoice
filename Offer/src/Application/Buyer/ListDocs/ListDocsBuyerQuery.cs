///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Offer.ListDocs;

namespace yourInvoice.Offer.Application.Buyer.ListDocs;
public record ListDocsBuyerQuery(int numberOffer) : IRequest<ErrorOr<List<ListDocsResponse>>>;