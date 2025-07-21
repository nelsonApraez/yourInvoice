///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Admin.SendSummary;

public record SendSummaryQuery(int offerId, List<string> emailsSeller) : IRequest<ErrorOr<bool>>;