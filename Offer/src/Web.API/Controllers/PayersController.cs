///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Payers.GetByNit;

namespace yourInvoice.Offer.Web.API.Controllers
{
    [Route("api/payer")]
    [ApiController]
    public class Payers : ApiController
    {
        private readonly ISender _mediator;

        public Payers(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("{nit}")]
        public async Task<IActionResult> GetAllPayerByNitAsync(string nit)
        {
            var payerResult = await _mediator.Send(new GetPayerByNitQuery(nit));

            return payerResult.Match(
                payer => Ok(payer),
                errors => Problem(errors)
            );
        }
    }
}