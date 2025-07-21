///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.HistoricalStates
{
    public interface IHistoricalStatesRepository
    {
        Task<bool> AddAsync(HistoricalState historicalState);

        Task<bool> AddListAsync(List<HistoricalState> historicalStates);
    }
}