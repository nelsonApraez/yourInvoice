///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Admin.UploadAttachment;
public record UploadAttachmentCommand(int OfferId, string FileBase64, string FileName) : IRequest<ErrorOr<bool>>;