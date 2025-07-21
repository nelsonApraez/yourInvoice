///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.InvoiceEvents
{
    public interface IInvoiceEventRepository
    {
        void Add(InvoiceEvent invoiceEvent);

        void Update(InvoiceEvent invoiceEvent);

        void Delete(Guid invoiceEventId);

        Task NullyfyAsync(Guid offerId);

        Task DeleteAsync(List<Guid> invoiceIds);
    }
}