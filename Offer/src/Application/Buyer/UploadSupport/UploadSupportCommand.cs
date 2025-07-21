///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Buyer.UploadSupport;

public record UploadSupportCommand(Guid OfferId, string FileBase64, string FileName) : IRequest<ErrorOr<bool>>;