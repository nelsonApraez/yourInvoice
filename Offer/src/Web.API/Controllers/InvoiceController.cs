///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Offer.Invoice.DeleteByIds;
using yourInvoice.Offer.Application.Offer.Invoice.SaveChangeConfirm;

namespace yourInvoice.Offer.Web.API.Controllers
{
    [Route("api/invoice")]
    public class InvoiceController : ApiController
    {
        private readonly ISender _mediator;

        public InvoiceController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> DeleteAsync(List<Guid> invoiceIds)
        {
            var deleteResult = await _mediator.Send(new DeleteOfferInvoiceByIdsCommand(invoiceIds));

            return deleteResult.Match(
                deleteOfferId => Ok(deleteOfferId),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("input")]
        public async Task<IActionResult> SaveChangeConfirmAsync(InvoiceExcelValidatedRequest request)
        {
            var saveInvoice = await _mediator.Send(new SaveChangeConfirmCommand(request));

            return saveInvoice.Match(
                resultSaveInvoice => Ok(resultSaveInvoice),
                errors => Problem(errors)
            );
        }
    }
}