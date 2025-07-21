///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.DianFyM.GetFileFtpDian;

public record GetFileFtpDianQuery(string userId) : IRequest<ErrorOr<IEnumerable<string>>>;