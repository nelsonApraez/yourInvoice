///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Extension;
using yourInvoice.Common.Constant;
using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Offer.Create;
using yourInvoice.Offer.Application.Offer.Detail;
using yourInvoice.Offer.Application.Offer.Generate;
using yourInvoice.Offer.Application.Offer.GetBase64Document;
using yourInvoice.Offer.Application.Offer.GetById;
using yourInvoice.Offer.Application.Offer.Invoice.ConfirmDowloadExcel;
using yourInvoice.Offer.Application.Offer.Invoice.List;
using yourInvoice.Offer.Application.Offer.Invoice.ListConfirm;
using yourInvoice.Offer.Application.Offer.Invoice.ListEvents;
using yourInvoice.Offer.Application.Offer.Invoice.Nullify;
using yourInvoice.Offer.Application.Offer.Invoice.Progress;
using yourInvoice.Offer.Application.Offer.Invoice.UploadFiles;
using yourInvoice.Offer.Application.Offer.Invoice.ValidateInvoicesExcel;
using yourInvoice.Offer.Application.Offer.ListAll;
using yourInvoice.Offer.Application.Offer.ListDocs;
using yourInvoice.Offer.Application.Offer.ListSellerDocs;
using yourInvoice.Offer.Application.Offer.SignDocs;
using yourInvoice.Offer.Application.Offer.SignSuccessDocs;
using yourInvoice.Offer.Application.Offer.ValidateDian;
using System.ComponentModel.DataAnnotations;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Web.API.Controllers
{
    [Route("api/offer")]
    public class OfferController : ApiController
    {
        private readonly ISender _mediator;

        public OfferController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Route("{offerId}/sign/success")]
        public async Task<IActionResult> SignSuccessDocs([Required] Guid offerId)
        {
            var result = await _mediator.Send(new SignSuccessDocsCommand(offerId));

            return result.Match(
              docs => Ok(docs),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("{offerId}/sign/confirm")]
        public async Task<IActionResult> SignDocs([Required] Guid offerId)
        {
            var result = await _mediator.Send(new SignDocsCommand(offerId));

            return result.Match(
              docs => Ok(docs),
              error => Problem(error)
            );
        }

        [HttpGet]
        [Route("sign/docs/{documentId}")]
        public async Task<IActionResult> GetBase64Document([Required] Guid documentId)
        {
            var result = await _mediator.Send(new GetBase64DocumentQuery(documentId));

            return result.Match(
              docs => Ok(new Dictionary<string, string> { { "base64", docs[0] }, { "type", docs[1] } }),
              error => Problem(error)
            );
        }

        [HttpGet]
        [Route("{offerId}/sign/docs")]
        public async Task<IActionResult> ListDocs([Required] Guid offerId)
        {
            var result = await _mediator.Send(new ListDocsQuery(offerId));

            return result.Match(
              docs => Ok(docs),
              error => Problem(error)
            );
        }

        [HttpGet]
        [Route("{offerId}/seller/document/list")]
        public async Task<IActionResult> ListSellerDocs([Required] Guid offerId)
        {
            var result = await _mediator.Send(new ListSellerDocsQuery(offerId));

            return result.Match(
              docs => Ok(docs),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("{offerId}/generate/docs")]
        public async Task<IActionResult> GenerateDocs([Required] Guid offerId)
        {
            var result = await _mediator.Send(new GenerateDocsCommand(offerId));

            return result.Match(
              result => Ok(result),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOfferCommand command)
        {
            var createResult = await _mediator.Send(command);

            return createResult.Match(
                createOfferResponse => Ok(createOfferResponse),
                errors => Problem(errors)
            );
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("{offerId}/invoice/load")]
        public async Task<IActionResult> LoadAsync([Required] Guid offerId, [FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return Problem(new List<Error> { Error.Validation(MessageCodes.FileRejectByNoSendFiles, GetErrorDescription(MessageCodes.FileRejectByNoSendFiles)) });

            var result = await _mediator.Send(new UploadFilesCommand(offerId, files));

            return result.Match(
                    uploadFilesResponse => Ok(uploadFilesResponse),
                    errors => Problem(errors));
        }

        [HttpPost]
        [Route("{offerId}/invoice/validate/negociation")]
        public async Task<IActionResult> ValidateInvoicesExcel([Required] Guid offerId, [FromBody] InvoicesFileRequest request)
        {
            var result = await _mediator.Send(new ValidateInvoicesExcelCommand(offerId, request.file));

            return result.Match(
              validateInvoices => Ok(validateInvoices),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("{offerId}/validate")]
        public async Task<IActionResult> ValidateDianAsync([Required] Guid offerId)
        {
            // obtiene las facturas que estan en cache.
            var result = await _mediator.Send(new ValidateDianCommand(offerId));
            //ErrorOr<bool> result = true;

            return result.Match(
                    validateDian => Ok(validateDian),
                    errors => Problem(errors)
                );
        }

        [HttpGet]
        [Route("{offerId}/invoice/progress")]
        public async Task<IActionResult> ProgressAsync([Required] Guid offerId)
        {
            // obtiene las facturas que estan en cache.
            var result = await _mediator.Send(new ProgressCommand(offerId));

            return result.Match(
                    invoiceProcessCache => Ok(invoiceProcessCache),
                    errors => Problem(errors)
                );
        }

        [HttpDelete]
        [Route("{offerId}")]
        public async Task<IActionResult> NullifyAsync(Guid offerId)
        {
            var nullifyResult = await _mediator.Send(new NullifyInvoceCommand(offerId));

            return nullifyResult.Match(
                 result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("{offerId}/invoice/list/paginate")]
        public async Task<IActionResult> ListAsync([Required] Guid offerId, SearchInfo pagination)
        {
            // obtiene las facturas que estan en cache.
            var result = await _mediator.Send(new ListInvoiceByOfferQuery(offerId, pagination));

            return result.Match(
                    invoiceListResponse => Ok(invoiceListResponse),
                    errors => Problem(errors)
                );
        }

        [HttpGet]
        [Route("{offerId}")]
        public async Task<IActionResult> GetAsync([Required] Guid offerId)
        {
            // obtiene las facturas que estan en cache.
            var result = await _mediator.Send(new GetOfferByIdQuery(offerId));

            return result.Match(
                    getOfferResponse => Ok(getOfferResponse),
                    errors => Problem(errors)
                );
        }

        [HttpPost]
        [Route("{offerId}/list/events/paginate")]
        public async Task<IActionResult> ListWithEventsAsync([Required] Guid offerId, SearchInfo pagination)
        {
            // obtiene las facturas que estan en cache.
            var result = await _mediator.Send(new ListInvoiceEventsByOfferQuery(offerId, pagination));

            return result.Match(
                    invoiceListEventsResponse => Ok(invoiceListEventsResponse),
                    errors => Problem(errors)
                );
        }

        [HttpPost]
        [Route("{offerId}/invoices/input/paginate")]
        public async Task<IActionResult> ListConfirmAsync([Required] Guid offerId, SearchInfo pagination)
        {
            var invoices = await _mediator.Send(new ListInvoiceConfirmByOfferQuery(offerId, pagination));

            return invoices.Match(
                    invoicesConfirmListResponse => Ok(invoicesConfirmListResponse),
                    errors => Problem(errors)
                );
        }

        [HttpGet]
        [Route("{offerId}/invoice/input/list")]
        public async Task<IActionResult> ListXlsxAsync([Required] Guid offerId)
        {
            string nameExcel = $"FACTURAS_{string.Format("{0:yyyyMMdd_HHmm}", ExtensionFormat.DateTimeCO())}.xlsx";
            var invoicesConfirm = await _mediator.Send(new ConfirmDowloadExcelQuery(offerId));
            return invoicesConfirm.Match(
                    invoices => File(invoices, Excel.TypeFile, nameExcel),
                    errors => Problem(errors)
                );
        }

        [HttpPost]
        [Route("list/paginate")]
        public async Task<IActionResult> ListAllOfferAsync(SearchInfo pagination)
        {
            var result = await _mediator.Send(new ListAllOfferQuery(pagination));
            return result.Match(
                    offerListResponse => Ok(offerListResponse),
                    errors => Problem(errors)
                );
        }

        [HttpGet]
        [Route("{offerId}/summary")]
        public async Task<IActionResult> DetailOfferAsync([Required] Guid offerId)
        {
            var result = await _mediator.Send(new DetailOfferQuery(offerId));
            return result.Match(
                    offerDetailResponse => Ok(offerDetailResponse),
                    errors => Problem(errors)
                );
        }
    }
}