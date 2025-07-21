///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Payers
{
    public interface IPayerRepository
    {
        void Add(Payer payer);

        void Update(Payer payer);

        void Delete(Guid payerId);

        Task<Payer> GetByIdAsync(Guid payerId);

        Task<List<Payer>> GetAllPayerByNitAsync(string nit);
    }
}