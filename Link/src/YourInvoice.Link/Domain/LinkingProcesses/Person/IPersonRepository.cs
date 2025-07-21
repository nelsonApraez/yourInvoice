///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.EF.Entity;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using System.Linq.Expressions;

namespace yourInvoice.Link.Domain.LinkingProcesses.Person
{
    public interface IPersonRepository
    {
        Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

        Task<IEnumerable<ExposureInformation>> GetExposureInformationAsync(Guid IdGeneralInformation);

        Task<IEnumerable<CatalogItemInfo>> GetCatalogItemsAsync(string catalogName);
        Task<GetDataLinkResponse> GetDataLinkNaturalUserAsync(Guid idUserLink);
        Task<GetDataLinkResponse> GetDataLinkLegalUserAsync(Guid idUserLink);
    }
}