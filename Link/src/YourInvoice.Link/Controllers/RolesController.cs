///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.AspNetCore.Mvc;
using yourInvoice.Common.Controller;

namespace yourInvoice.Link.Controllers
{
    [Route("api/roles")]
    public class RolesController : ApiController

    {
        private readonly ISender _mediator;

        public RolesController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
    }
}