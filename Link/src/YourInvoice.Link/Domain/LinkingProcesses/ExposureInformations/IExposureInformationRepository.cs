///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations
{
    public interface IExposureInformationRepository
    {
        Task<bool> ExistsExposureAsync(Guid idGeneralInformation);

        public Task<bool> UpdateExposureAsync(ExposureInformation exposure);

        public Task<GetExposureResponse> GetExposureAsync(Guid idGeneralInformation);

        public Task<bool> CreateExposureAsync(IEnumerable<ExposureInformation> exposure);

        public Task<List<GetExposureQuestionsAnswerResponse>> GetExposureQuestionAnswerAsync(string catalogName, List<Guid> showDetail);

        Task<ListDataInfo<ListEconomicActivityResponse>?> GetEconomicActitiesListAsync();
    }
}