///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Admin.DeleteAttachment;

public record DeleteAttachmentCommand(Guid documentId) : IRequest<ErrorOr<bool>>;