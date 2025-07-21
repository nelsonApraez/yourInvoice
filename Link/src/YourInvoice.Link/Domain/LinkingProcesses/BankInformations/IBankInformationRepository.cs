///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Domain.LinkingProcesses.BankInformations
{
    public interface IBankInformationRepository
    {
        Task<bool> ExistsBankAsync(Guid idGeneralInformation);

        public Task<bool> CreateBankAsync(BankInformation bank);


        public Task<bool> UpdateBankAsync(BankInformation bank);

        public Task<GetBankResponse> GetbankInformationAsync(Guid Id);

    }
}
