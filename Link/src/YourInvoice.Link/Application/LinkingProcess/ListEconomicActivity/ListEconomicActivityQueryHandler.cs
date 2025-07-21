///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.ListEconomicActivity
{
    public sealed class ListEconomicActivityQueryHandler : IRequestHandler<ListEconomicActivityQuery, ErrorOr<ListDataInfo<ListEconomicActivityResponse>>>
    {
        private readonly IExposureInformationRepository repository;
        private readonly string orderDefault = "statusid";
        private readonly string fieldOrder = "name";
        private readonly string fieldOrderChange = "NameOrder";

        public ListEconomicActivityQueryHandler(IExposureInformationRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<ListDataInfo<ListEconomicActivityResponse>>> Handle(ListEconomicActivityQuery query, CancellationToken cancellationToken)
        {
            var economicActivity = await repository.GetEconomicActitiesListAsync();
            if (economicActivity is not null && economicActivity.Data?.Count > 0)
            {
                var data = economicActivity.Data.OrderBy(o => o.OrderRegister).ThenByDescending(o => o.CreatedOn);
                return new ListDataInfo<ListEconomicActivityResponse>
                {
                    Count = economicActivity.Count,
                    Data = data.ToList(),
                };
            }
            return economicActivity ?? new ListDataInfo<ListEconomicActivityResponse>();
        }
    }
}