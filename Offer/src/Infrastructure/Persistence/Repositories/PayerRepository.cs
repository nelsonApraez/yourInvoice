///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Payers;

namespace yourInvoice.Offer.Infrastructure.Persistence.Repositories
{
    public class PayerRepository : IPayerRepository
    {
        private readonly ApplicationDbContext _context;

        public PayerRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(Payer payer)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid payerId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Payer>> GetAllPayerByNitAsync(string nit)
        {
            var result = await _context.Payers.Where(c => c.Nit.StartsWith(nit)).ToListAsync();

            return result;
        }

        public void Update(Payer payer)
        {
            throw new NotImplementedException();
        }

        public async Task<Payer> GetByIdAsync(Guid payerId) => await _context.Payers.SingleOrDefaultAsync(c => c.Id == payerId);
    }
}