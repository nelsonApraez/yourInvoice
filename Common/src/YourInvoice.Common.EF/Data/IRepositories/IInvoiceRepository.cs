///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Common.EF.Entity;
using yourInvoice.Common.Entities;

namespace yourInvoice.Common.EF.Data.IRepositories
{
    public interface IInvoiceRepository : IRepository<InvoiceInfo>
    {
        Task<bool> UpdateStateInvoiceAsync(List<InvoiceInfo> invoiceIds);

        List<ResumeInvoicesProcessDian> GetInvoicesProcessed(Guid offerId, int consecutive = 0);

        Task<string> GetEmailUserByOffer(Guid offerId, int consecutive = 0);

        Task<bool> UpdateStateInvoiceProcessDianAsync(Guid offerId);
        Task<bool> ExistsStatusWaitValidationDianAsync(Guid offerId, Guid statusId);
    }
}