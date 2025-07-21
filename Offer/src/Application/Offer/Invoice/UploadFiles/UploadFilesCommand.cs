///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.AspNetCore.Http;

namespace yourInvoice.Offer.Application.Offer.Invoice.UploadFiles
{
    public record UploadFilesCommand(Guid offerId, List<IFormFile> files) : IRequest<ErrorOr<UploadFilesResponse>>;
}