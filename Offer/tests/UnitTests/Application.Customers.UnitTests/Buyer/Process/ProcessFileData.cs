///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Buyer.ProcessFile;
using yourInvoice.Offer.Domain.InvoiceDispersions.Queries;
using yourInvoice.Offer.Domain.OperationFiles;

namespace Application.Customer.UnitTest.Buyer.Process
{
    public static class ProcessFileData
    {
        public static CatalogItemInfo GetCatalogItemInfo => new CatalogItemInfo
        {
            Name = "Fecha de expiracion",
            Descripton = "fecha de expiracion en horas",
            CatalogName = "24",
        };

        public static List<OperationFile> GeOperationFile => new List<OperationFile>
        {
            new OperationFile( id:Guid.NewGuid(),name:"COMP_OF-370_800099903_20240130_214129.xlsx",description:"archivo ftp",
                                startDate:DateTime.Now,endDate:DateTime.Now,status:false,createdOn:DateTime.Now,
                                createdBy:Guid.NewGuid(),modifiedOn:DateTime.Now,modifiedBy:Guid.NewGuid())
        };

        public static List<PurchaseOperation> GetPurchaseOperation => new List<PurchaseOperation>()
        {
            new PurchaseOperation
            {
                 BuyerId = Guid.NewGuid(),
                  DiasOper=6,
                   DineroNuevo="NO",
                    FacturaNo="FC001",
                     FechaDeCompra=DateTime.Now,
                      FechaFinal=DateTime.Now,
                       Fraccionamiento="01",
                        InvoiceDispersionId=Guid.NewGuid(),
                         NombreComprador="Nombre del comprador",
                          NombrePagador="Nombre del pagador",
                           NombreVendedor="Nombre del vendedor",
                            NoTran=1,
                             NumeroDeDocumentoComprador="111",
                              NumeroDeDocumentoPagador="222",
                               NumeroDeDocumentoVendedor="333",
                                PayerId=Guid.NewGuid(),
                                 Reasignacion="I",
                                  SellerId=Guid.NewGuid(),
                                   TasaEAPorcentaje=17,
                                    TipoDeDocumentoComprador="C",
                                     VrCompra=1000000,
                                      VrFuturo=200000
            }
        };

        public static SearchInfo GeSearchInfo => new SearchInfo();

        public static ListDataInfo<ResumeInvoiceResponse> GetResumeInvoiceResponseEmpty => new ListDataInfo<ResumeInvoiceResponse>();

        public static int NumberOffer => 370;

        public static Guid UserId => Guid.Empty;
    }
}