///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Domain.LinkingProcesses.FinancialInformations
{
    public interface IFinancialInformationRepository
    {
        Task<bool> ExistsFinancialAsync(Guid idGeneralInformation);

        public Task<bool> CreateFinancialAsync(FinancialInformation financial);

        public Task<bool> UpdateFinancialAsync(FinancialInformation financial);

        public Task<GetFinancialResponse> GetFinancialInformationAsync(Guid Id);

    }
}