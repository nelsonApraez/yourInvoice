///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.AspNetCore.Mvc;
using yourInvoice.Common.Controller;
using yourInvoice.Common.Entities;
using yourInvoice.Link.Application.Accounts.Approve;
using yourInvoice.Link.Application.Accounts.CreateAccount;
using yourInvoice.Link.Application.Accounts.GetAccount;
using yourInvoice.Link.Application.Accounts.List;
using yourInvoice.Link.Application.Accounts.Reject;
using yourInvoice.Link.Application.Accounts.Validity;
using System.ComponentModel.DataAnnotations;

namespace yourInvoice.Link.Controllers
{
    [Route("api/account")]
    public class AccountController : ApiController
    {
        private readonly ISender _mediator;

        public AccountController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("validity")]
        public async Task<IActionResult> Validity()
        {
            var result = await _mediator.Send(new ValidityQuery());

            return result.Match(
              header => Ok(header),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAccountCommand command)
        {
            var createResult = await _mediator.Send(command);

            return createResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([Required] Guid Id)
        {
            var result = await _mediator.Send(new GetAccountQuery(Id));
            return result.Match(
              header => Ok(header),
              error => Problem(error)
            );
        }

        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> ListAsync(SearchInfo pagination)
        {
            var result = await _mediator.Send(new ListQuery(pagination));
            return result.Match(
              list => Ok(list),
              error => Problem(error)
            );
        }


        [HttpPost]
        [Route("reject/{id}")]
        public async Task<IActionResult> RejectAsync([Required] Guid Id)
        {
            var createResult = await _mediator.Send(new RejectAccountCommand(Id));

            return createResult.Match(
                beneficiaryId => Ok(beneficiaryId),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("approve/{Id}")]
        public async Task<IActionResult> ApproveAsync([Required] Guid Id)
        {
            var createResult = await _mediator.Send(new ApproveAccountCommand(Id));

            return createResult.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }

    }
}