///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Beneficiary.Create;
using yourInvoice.Offer.Application.Beneficiary.Delete;
using yourInvoice.Offer.Application.Beneficiary.List;
using System.ComponentModel.DataAnnotations;

namespace yourInvoice.Offer.Web.API.Controllers
{
    [Route("api/beneficiary")]
    public class BeneficiaryController : ApiController
    {
        private readonly ISender _mediator;

        public BeneficiaryController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBeneficiaryCommand command)
        {
            var createResult = await _mediator.Send(command);

            return createResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("{offerId}/list/paginate")]
        public async Task<IActionResult> ListAsync([Required] Guid offerId, SearchInfo pagination)
        {
            //obtiene los beneficiarios
            var result = await _mediator.Send(new ListBeneficiariesQuery(offerId, pagination));

            return result.Match(
                    beneficiaryListResponse => Ok(beneficiaryListResponse),
                    errors => Problem(errors)
                );
        }

        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> DeleteAsync(List<Guid> beneficiaryIds)
        {
            var result = await _mediator.Send(new DeleteBeneficiaryCommand(beneficiaryIds));

            return result.Match(
                 result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}