///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Catalog.GetByName;
using System.ComponentModel.DataAnnotations;
using yourInvoice.Offer.Application.Catalog.GetByNames;
using yourInvoice.Offer.Application.Catalog.GetByParentId;
using yourInvoice.Offer.Application.Catalog.GetCatalogOrderDescription;

namespace yourInvoice.Offer.Web.API.Controllers
{
    [Route("api/catalog")]
    public class CatalogController : ApiController
    {
        private readonly ISender _mediator;

        public CatalogController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("{catalogName}")]
        public async Task<IActionResult> GetAsync([Required] string catalogName)
        {
            // obtiene las facturas que estan en cache.
            var result = await _mediator.Send(new GetCatalogByNameQuery(catalogName));

            return result.Match(
                    catalogItemInfo => Ok(catalogItemInfo),
                    errors => Problem(errors)
                );
        }

        [HttpPost]
        [Route("catalogs")]
        public async Task<IActionResult> GetByNamesAsync([FromBody] string[] names)
        {
            // obtiene las facturas que estan en cache.
            var result = await _mediator.Send(new GetByNamesQuery(names));

            return result.Match(
                catalogItemInfo => Ok(catalogItemInfo),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        [Route("GetByParentId/{parentId}")]
        public async Task<IActionResult> GetByParentIdAsync([Required] Guid parentId)
        {
            var result = await _mediator.Send(new GetCatalogByParentIdQuery(parentId));
            return result.Match(
                    catalogItemInfo => Ok(catalogItemInfo),
                    errors => Problem(errors)
                );
        }

        [HttpGet]
        [Route("GetCatalogOrderDescription/{catalogName}")]
        public async Task<IActionResult> GetCatalogOrderDescriptionAsync([Required] string catalogName)
        {
            var result = await _mediator.Send(new GetCatalogOrderDescriptionQuery(catalogName));

            return result.Match(
                    catalogItemInfo => Ok(catalogItemInfo),
                    errors => Problem(errors)
                );
        }
    }
}