///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT
{
    public interface ILegalSAGRILAFTRepository
    {
        Task<bool> ExistsSagrilaftAsync(Guid idLegalGeneralInformation);

        public Task<bool> UpdateSagrilaftAsync(LegalSAGRILAFT sagrilaft);

        public Task<GetSagrilaftResponse> GetSagrilaftAsync(Guid idLegalGeneralInformation);

        public Task<bool> CreateSagrilaftAsync(IEnumerable<LegalSAGRILAFT> sagrilaft);

        public Task<List<GetSagrilaftQuestionsAnswerResponse>> GetSagrilaftQuestionAnswerAsync(string catalogName, List<Guid> showDetail);

        Task<ListDataInfo<ListEconomicActivityResponse>?> GetEconomicActitiesListAsync();
    }
}