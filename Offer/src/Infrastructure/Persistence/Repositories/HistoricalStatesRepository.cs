///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.HistoricalStates;

namespace yourInvoice.Offer.Infrastructure.Persistence.Repositories
{
    public class HistoricalStatesRepository : IHistoricalStatesRepository
    {
        private readonly ApplicationDbContext _context;

        public HistoricalStatesRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AddAsync(HistoricalState historicalState)
        {
            await _context.HistoricalStates.AddAsync(historicalState);
            return true;
        }

        public async Task<bool> AddListAsync(List<HistoricalState> historicalStates)
        {
            await _context.HistoricalStates.AddRangeAsync(historicalStates);
            return true;
        }
    }
}