///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Buyer.GetFileFtp;
public record GetFileFtpQuery(string userId) : IRequest<ErrorOr<IEnumerable<string>>>;