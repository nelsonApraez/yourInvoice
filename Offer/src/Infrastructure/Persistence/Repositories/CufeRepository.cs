///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Cufes;

namespace yourInvoice.Offer.Infrastructure.Persistence.Repositories
{
    public class CufeRepository : ICufeRepository
    {
        private readonly ApplicationDbContext _context;

        public CufeRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(Cufe cufe)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid cufeId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsByCufeAsync(string cufe) => await _context.Cufes.AnyAsync(cuf => cuf.CufeValue == cufe);
    }
}