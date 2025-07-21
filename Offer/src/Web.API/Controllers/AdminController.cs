///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Admin.DeleteAttachment;
using yourInvoice.Offer.Application.Admin.Header;
using yourInvoice.Offer.Application.Admin.HeaderOffer;
using yourInvoice.Offer.Application.Admin.HeaderTransaction;
using yourInvoice.Offer.Application.Admin.ListDetail;
using yourInvoice.Offer.Application.Admin.ListDocs;
using yourInvoice.Offer.Application.Admin.ListPending;
using yourInvoice.Offer.Application.Admin.ListPurchased;
using yourInvoice.Offer.Application.Admin.ListResumeDocs;
using yourInvoice.Offer.Application.Admin.ListTransactions;
using yourInvoice.Offer.Application.Admin.SendSummary;
using yourInvoice.Offer.Application.Admin.UploadAttachment;
using yourInvoice.Offer.Application.Buyer.UploadSupport;
using System.ComponentModel.DataAnnotations;

namespace yourInvoice.Offer.Web.API.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ApiController
    {
        private readonly ISender _mediator;

        public AdminController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("offer/transaction/{transactionId}/header")]
        public async Task<IActionResult> GetHeaderTransaction([Required] int transactionId)
        {
            var result = await _mediator.Send(new HeaderTransactionQuery(transactionId));

            return result.Match(
              header => Ok(header),
              error => Problem(error)
            );
        }

        [HttpGet]
        [Route("offer/transaction/{transactionId}/list")]
        public async Task<IActionResult> ListTransactions([Required] int transactionId)
        {
            var result = await _mediator.Send(new ListTransactionsQuery(transactionId));//result

            return result.Match(
              header => Ok(header),
              error => Problem(error)
            );
        }

        [HttpGet]
        [Route("offer/{transactionId}/document/list")]
        public async Task<IActionResult> ListDocs([Required] int transactionId)
        {
            var result = await _mediator.Send(new ListAdminDocsQuery(transactionId));

            return result.Match(
              docs => Ok(docs),
              error => Problem(error)
            );
        }

        [HttpGet]
        [Route("offer/{offerId}/resume/vendor")]
        public async Task<IActionResult> GetHeaderOffer([Required] int offerId)
        {
            var result = await _mediator.Send(new HeaderOfferQuery(offerId));

            return result.Match(
              header => Ok(header),
              error => Problem(error)
            );
        }

        [HttpGet]
        [Route("offer/{offerId}/resume/document/list")]
        public async Task<IActionResult> ListResumeDocs([Required] int offerId)
        {
            var result = await _mediator.Send(new ListResumeDocsQuery(offerId));

            return result.Match(
              docs => Ok(docs),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("offer/{offerId}/resume/attachment")]
        public async Task<IActionResult> UploadAttachment([Required] int offerId, [FromBody] PaymentSupportFileRequest request)
        {
            var result = await _mediator.Send(new UploadAttachmentCommand(offerId, request.file, request.fileName));

            return result.Match(
              result => Ok(result),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("offer/pending/list")]
        public async Task<IActionResult> PendingListAsync(SearchInfo pagination)
        {
            var result = await _mediator.Send(new ListPendingQuery(pagination));

            return result.Match(
              result => Ok(result),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("offer/purchased/list")]
        public async Task<IActionResult> PurchasedListAsync(SearchInfo pagination)
        {
            var result = await _mediator.Send(new ListPurchasedQuery(pagination));

            return result.Match(
              result => Ok(result),
              error => Problem(error)
            );
        }

        [HttpGet]
        [Route("offer/{offerId}/header")]
        public async Task<IActionResult> HeaderAsync([Required] int offerId)
        {
            var result = await _mediator.Send(new HeaderDetailQuery(offerId));

            return result.Match(
              result => Ok(result),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("offer/{offerId}/detail")]
        public async Task<IActionResult> ListDetailAsync([Required] int offerId, SearchInfo pagination)
        {
            var result = await _mediator.Send(new ListDetailQuery(offerId, pagination));

            return result.Match(
              result => Ok(result),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("offer/{offerId}/resume/sendmail")]
        public async Task<IActionResult> ResumeSendMailAsync([Required] int offerId, [Required] List<string> emailsSeller)
        {
            var result = await _mediator.Send(new SendSummaryQuery(offerId, emailsSeller));

            return result.Match(
              result => Ok(result),
              error => Problem(error)
            );
        }

        [HttpDelete]
        [Route("offer/resume/{documentId}/attachment")]
        public async Task<IActionResult> DeleteAttachmentAsync([Required] Guid documentId)
        {
            var result = await _mediator.Send(new DeleteAttachmentCommand(documentId));

            return result.Match(
              result => Ok(result),
              error => Problem(error)
            );
        }
    }
}