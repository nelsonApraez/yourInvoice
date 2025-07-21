///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Cufes
{
    public interface ICufeRepository
    {
        void Add(Cufe cufe);

        void Delete(Guid cufeId);

        Task<bool> ExistsByCufeAsync(string cufe);
    }
}