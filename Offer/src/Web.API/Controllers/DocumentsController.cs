///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Documents.Common;
using yourInvoice.Offer.Application.Documents.GetDocumentsByOfferInvoice;

namespace yourInvoice.Offer.Web.API.Controllers
{
    [Route("api/document")]
    [ApiController]
    public class DocumentsController : ApiController
    {
        private readonly ISender _mediator;

        public DocumentsController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Route("download")]
        public async Task<IActionResult> GetDocumentsByOfferInvoiceAsync(DocumentByOfferInvoiceRequest request)
        {
            var payerResult = await _mediator.Send(new GetDocumentsByOfferInvoiceQuery(request));

            return payerResult.Match(
                document => Ok(document),
                errors => Problem(errors)
            );
        }
    }
}