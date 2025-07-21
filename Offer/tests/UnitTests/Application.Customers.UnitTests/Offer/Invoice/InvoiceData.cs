///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Offer.Invoice.SaveChangeConfirm;
using yourInvoice.Offer.Domain.Invoices.Queries;
using InvoiceUsing = yourInvoice.Offer.Domain.Invoices;

namespace Application.Customer.UnitTest.Offer.Invoice
{
    public static class InvoiceData
    {
        public static ListDataInfo<InvoiceListConfirmDataResponse> GetInvoiceListConfirmDataResponse => new ListDataInfo<InvoiceListConfirmDataResponse>()
        {
            Count = 1,
            Data = new List<InvoiceListConfirmDataResponse>
              {
                   new InvoiceListConfirmDataResponse{ Nro=1, ConsecutiveOffer=555, DueDate="04/Mar/2023", InvoiceNumber="FEVN6445", Status="", Value=202000, ValueIva=1 }
              },
        };

        public static ListDataInfo<InvoiceListConfirmDataResponse> GetInvoiceListConfirmDataResponseEmpy => new ListDataInfo<InvoiceListConfirmDataResponse>();

        public static ListDataInfo<InvoiceListConfirmDataResponse> GetInvoiceListConfirmDataResponseNull => null;

        public static List<InvoiceExcelValidated> GetInvoiceExcelValidated => new List<InvoiceExcelValidated> { new InvoiceExcelValidated { Fecha_de_Pago = DateTime.Now, Numero_Factura = "FNFEN45", Valor_de_Pago_Neto = 200333 } };

        public static List<InvoiceExcelValidated> GetInvoiceExcelValidatedNull => null;

        public static IEnumerable<InvoiceUsing.Invoice> GetInvoices => new List<InvoiceUsing.Invoice> {
            new InvoiceUsing.Invoice(Guid.NewGuid(), Guid.NewGuid(), "FNFEN45", "zipname", "KSJKDFDFDDFDFDFD", Guid.NewGuid(), DateTime.Now, DateTime.Now,300000,1,Guid.NewGuid(), 23, "", DateTime.Now, 2323232323)
        };
    }
}