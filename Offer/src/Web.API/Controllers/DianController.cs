///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.AspNetCore.Authorization;
using yourInvoice.Offer.Application.DianFyM.GetFileFtpDian;
using yourInvoice.Offer.Application.DianFyM.ProcessFileDian;

namespace yourInvoice.Offer.Web.API.Controllers
{
    [Route("api/dian")]
    [ApiController]
    public class DianController : ApiController
    {
        private readonly ISender _mediator;

        public DianController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Route("getfileftpdian")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFileFtpAsync()
        {
            var nameFileFtp = await _mediator.Send(new GetFileFtpDianQuery(""));
            return nameFileFtp.Match(
                nameFiles => Ok(nameFiles),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [Route("process/filedian")]
        [AllowAnonymous]
        public async Task<IActionResult> FileAsync()
        {
            var processFile = await _mediator.Send(new ProcessFileDianCommand());
            return processFile.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}