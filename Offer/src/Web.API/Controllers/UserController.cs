///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Offer.Application.User.Create;
using yourInvoice.Offer.Application.User.GetRole;

namespace yourInvoice.Offer.Web.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ApiController
    {
        private readonly ISender _mediator;

        public UserController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("role")]
        public async Task<IActionResult> GetRoleAsync()
        {
            var result = await _mediator.Send(new GetRoleQuery());

            return result.Match(
                response => Ok(response),
                errors => Problem(errors)
            );
        }
    }
}