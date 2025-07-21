///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Offer.ListDocs;

namespace yourInvoice.Offer.Application.Admin.ListResumeDocs;
public record ListResumeDocsQuery(int offerId) : IRequest<ErrorOr<List<ListDocsResponse>>>;