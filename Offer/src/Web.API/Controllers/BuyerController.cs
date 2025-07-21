///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.AspNetCore.Authorization;
using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Buyer.Expired;
using yourInvoice.Offer.Application.Buyer.GenerateDocs;
using yourInvoice.Offer.Application.Buyer.GetFileFtp;
using yourInvoice.Offer.Application.Buyer.ListDocs;
using yourInvoice.Offer.Application.Buyer.ListOffers;
using yourInvoice.Offer.Application.Buyer.ProcessFile;
using yourInvoice.Offer.Application.Buyer.Reject;
using yourInvoice.Offer.Application.Buyer.Resume;
using yourInvoice.Offer.Application.Buyer.ResumeInvoice;
using yourInvoice.Offer.Application.Buyer.SignDocs;
using yourInvoice.Offer.Application.Buyer.SignSuccessDocs;
using yourInvoice.Offer.Application.Buyer.UploadSupport;
using yourInvoice.Offer.Application.Offer.GetBase64Document;
using System.ComponentModel.DataAnnotations;

namespace yourInvoice.Offer.Web.API.Controllers
{
    [Route("api/buyer")]
    [ApiController]
    public class BuyerController : ApiController
    {
        private readonly ISender _mediator;

        public BuyerController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Route("offer/{numberOffer}/sign/success")]
        public async Task<IActionResult> SignSuccessDocs([Required] int numberOffer)
        {
            var result = await _mediator.Send(new SignSuccessDocsBuyerCommand(numberOffer));
            return result.Match(
              docs => Ok(docs),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("offer/{numberOffer}/sign/confirm")]
        public async Task<IActionResult> SignDocs([Required] int numberOffer)
        {
            var result = await _mediator.Send(new SignDocsBuyerCommand(numberOffer));
            return result.Match(
              docs => Ok(docs),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("offer/{numberOffer}/generate/docs")]
        public async Task<IActionResult> GenerateDocs([Required] int numberOffer)
        {
            var result = await _mediator.Send(new GenerateDocsBuyerCommand(numberOffer));
            return result.Match(
              result => Ok(result),
              error => Problem(error)
            );
        }

        [HttpGet]
        [Route("offer/{numberOffer}/sign/docs")]
        public async Task<IActionResult> ListDocs([Required] int numberOffer)
        {
            var result = await _mediator.Send(new ListDocsBuyerQuery(numberOffer));
            return result.Match(
              docs => Ok(docs),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("list/paginate")]
        public async Task<IActionResult> ListOffersAsync(SearchInfo pagination)
        {
            // obtiene las facturas que estan en cache.
            var result = await _mediator.Send(new ListOffersByBuyerQuery(pagination, false));

            return result.Match(
                    listResponse => Ok(listResponse),
                    errors => Problem(errors)
                );
        }

        [HttpPost]
        [Route("history/list/paginate")]
        public async Task<IActionResult> ListOffersHistoryAsync(SearchInfo pagination)
        {
            var result = await _mediator.Send(new ListOffersByBuyerQuery(pagination, true));
            return result.Match(
                    listResponse => Ok(listResponse),
                    errors => Problem(errors)
                );
        }

        [HttpPost]
        [Route("offer/{offerId}/upload/support/transfer")]
        public async Task<IActionResult> UploadSupport([Required] Guid offerId, [FromBody] PaymentSupportFileRequest request)
        {
            var result = await _mediator.Send(new UploadSupportCommand(offerId, request.file, request.fileName));
            return result.Match(
              result => Ok(result),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("getfileftp")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFileFtpAsync()
        {
            var nameFileFtp = await _mediator.Send(new GetFileFtpQuery(""));
            return nameFileFtp.Match(
                nameFiles => Ok(nameFiles),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("review/expired")]
        [AllowAnonymous]
        public async Task<IActionResult> ExpiredAsync()
        {
            var processExpired = await _mediator.Send(new ExpiredCommand());
            return processExpired.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("process/file")]
        [AllowAnonymous]
        public async Task<IActionResult> FileAsync()
        {
            var processFile = await _mediator.Send(new ProcessFileCommand());
            return processFile.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("reminder")]
        [AllowAnonymous]
        public async Task<IActionResult> ReminderAsync()
        {
            _ = await Task.Run(() =>
            {
                return Ok("Por implementar");
            });
            return Ok("Por implementar");
        }

        [HttpGet]
        [Route("offer/{numberOffer}/resume")]
        public async Task<IActionResult> ResumeAsync(int numberOffer)
        {
            var resume = await _mediator.Send(new ResumeQuery(numberOffer));
            return resume.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("offer/{numberOffer}/resume/invoice")]
        public async Task<IActionResult> ResumeInvoiceAsync(SearchInfo pagination, int numberOffer)
        {
            var resumeInvoice = await _mediator.Send(new ResumeInvoiceQuery(pagination, numberOffer));
            return resumeInvoice.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("{numberOffer}/buy/reject")]
        public async Task<IActionResult> RejectOfferAsync([Required] int numberOffer)
        {
            var resumeInvoice = await _mediator.Send(new RejectCommand(numberOffer));
            return resumeInvoice.Match(
                result => Ok(result),
                errors => Problem(errors)
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
    }
}