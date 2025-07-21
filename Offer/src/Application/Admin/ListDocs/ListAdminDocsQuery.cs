///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Offer.ListDocs;

namespace yourInvoice.Offer.Application.Admin.ListDocs;
public record ListAdminDocsQuery(int transactionId) : IRequest<ErrorOr<Dictionary<string, List<ListDocsResponse>>>>;