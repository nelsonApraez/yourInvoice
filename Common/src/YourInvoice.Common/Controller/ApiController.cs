///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using yourInvoice.Common.Controller.Common.Http;
using System.Web.Http;

namespace yourInvoice.Common.Controller
{
    [Authorize]
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected IActionResult Problem(List<Error> ers)
        {
            if (ers.Count is 0)
            {
                return Problem();
            }

            if (ers.All(error => error.Type == ErrorType.Validation))
            {
                return ValidationProblem(ers);
            }

            HttpContext.Items[HttpContextItemKeys.Erros] = ers;

            return Problem(ers[0]);
        }

        private IActionResult Problem(Error err)
        {
            var statusCode = err.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError,
            };

            return Problem(statusCode: statusCode, title: err.Description);
        }

        private IActionResult ValidationProblem(List<Error> err)
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in err)
            {
                modelStateDictionary.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem(modelStateDictionary);
        }
    }
}