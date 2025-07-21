///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using MediatR;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Integration.FtpFiles;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Common.Integration.ZapSign;
using yourInvoice.Offer.Application.HistoricalStates.Add;
using yourInvoice.Offer.Application.Offer.SignSuccessDocs;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.HistoricalStates;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Invoices.Queries;
using yourInvoice.Offer.Domain.Notifications;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Offer.SignSuccessDocs
{
    public class SignSuccessDocsCommandHandlerTest
    {
        private readonly Mock<IOfferRepository> _mockRepository;
        private readonly Mock<IZapsign> _mockZapsign;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IDocumentRepository> _mockDocumentRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IStorage> _mockStorage;
        private readonly Mock<ICatalogBusiness> _mockCatalogBusiness;
        private readonly Mock<IInvoiceRepository> _invoiceRepository;
        private readonly Mock<IFtp> _ftp;
        private readonly Mock<IHistoricalStatesRepository> _mockIHistoricalStatesRepository;
        private readonly Mock<IMediator> _mockIMediator;
        private readonly SignSuccessDocsCommandHandler _handler;

        public SignSuccessDocsCommandHandlerTest()
        {
            _mockCatalogBusiness = new Mock<ICatalogBusiness>();
            _mockRepository = new Mock<IOfferRepository>();
            _mockZapsign = new Mock<IZapsign>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockDocumentRepository = new Mock<IDocumentRepository>();
            _mockStorage = new Mock<IStorage>();
            _invoiceRepository = new Mock<IInvoiceRepository>();
            _ftp = new Mock<IFtp>();
            _mockIHistoricalStatesRepository = new Mock<IHistoricalStatesRepository>();
            _mockIMediator = new Mock<IMediator>();
            _handler = new SignSuccessDocsCommandHandler(_mockUnitOfWork.Object, _mockStorage.Object, _mockRepository.Object,
                _mockZapsign.Object, _mockUserRepository.Object, _mockDocumentRepository.Object, _mockCatalogBusiness.Object, _invoiceRepository.Object, _ftp.Object,
                _mockIHistoricalStatesRepository.Object, _mockIMediator.Object);
        }

        [Fact]
        public async Task HandleSignSuccessDocs_NoToken_Error()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();
            Guid payerid = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            CatalogItemInfo EmailPort = new CatalogItemInfo() { Name = "EMAIL Port", Descripton = "587" };
            CatalogItemInfo EMAILServer = new CatalogItemInfo() { Name = "EMAIL Server", Descripton = "smtp.gmail.com" };
            CatalogItemInfo EMAILUser = new CatalogItemInfo() { Name = "EMAIL User", Descripton = "yourInvoicenotification@gmail.com" };
            CatalogItemInfo EMAILPasProjrd = new CatalogItemInfo() { Name = "EMAIL PasProjrd", Descripton = "ghkohgjmxfepfnbb" };
            CatalogItemInfo EMAILFrom = new CatalogItemInfo() { Name = "EMAIL From", Descripton = "yourInvoicenotification@gmail.com" };
            CatalogItemInfo EMAILSender = new CatalogItemInfo() { Name = "EMAIL Sender", Descripton = "yourInvoicenotification@gmail.com" };

            yourInvoice.Offer.Domain.Offer offer = new(offerid, payerid, userId, DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress);
            Document MoneyTransferInstruction = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.MoneyTransferInstruction, false, "url");
            Document CommercialOffer = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.CommercialOffer, false, "url");
            Document Endorsement = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.Endorsement, false, "url");
            Document EndorsementNotification = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.EndorsementNotification, false, "url");
            Document MoneyTransferInstructionExcel = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.MoneyTransferInstructionExcel, false, "url");

            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(new yourInvoice.Offer.Domain.Users.User(userId, 2, "890104521", "test name", Guid.NewGuid(), "72335847",
                "Cali", "Gerente", "Calle de prueba 123", "3015433443", "test@test.com", "Cali", Guid.NewGuid(), Guid.NewGuid(), "Licores del valle",
                "97654234", "5", "8725426273", "Cali", "Cali", true, DateTime.UtcNow, Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()));
            _mockRepository.Setup(x => x.GetByIdAsync(offerid)).ReturnsAsync(offer);
            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync("ruta");
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(offerid)).ReturnsAsync(new List<Document>() { MoneyTransferInstruction, CommercialOffer, Endorsement, EndorsementNotification, MoneyTransferInstructionExcel });
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "100" });
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(CatalogCode_DatayourInvoice.EmailAdmin)).ReturnsAsync(new CatalogItemInfo() { Descripton = "test@test.com" });
            _mockUserRepository.Setup(x => x.GetEmailRoleAsync(CatalogCode_UserRole.Administrator)).ReturnsAsync("test@test.com");

            _mockCatalogBusiness.Setup(x => x.ListByCatalogAsync("EMAILyourInvoice")).ReturnsAsync(new List<CatalogItemInfo>() { EmailPort, EMAILFrom, EMAILPasProjrd, EMAILSender, EMAILServer, EMAILUser });

            _mockIMediator.Setup(m => m.Send(It.IsAny<AddHistoricalCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Notification()).Verifiable("Notificacion enviada");
            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync(new MemoryStream());
            _invoiceRepository.Setup(x => x.ListToGenerateExcel(offerid)).Returns(new List<yourInvoice.Offer.Domain.Invoices.Queries.InvoiceListGenerateExcelResponse>()
            { new yourInvoice.Offer.Domain.Invoices.Queries.InvoiceListGenerateExcelResponse()
            {  } });

            _mockZapsign.Setup(x => x.GetDetailAsync(It.IsAny<string>())).ReturnsAsync(
                new ZapsignFileDetailResponse()
                {
                    extra_docs = new List<ZapsignFileDetailExtraDocResponse>() { new ZapsignFileDetailExtraDocResponse() { name = "Anexo.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" },
                            new ZapsignFileDetailExtraDocResponse() { name = "InstruccionGiro.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" },
                    new ZapsignFileDetailExtraDocResponse() { name = "EndosoDeDocumentos.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" },
                    new ZapsignFileDetailExtraDocResponse() { name = "NotificacionEndoso.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" }},
                    signed_file = "http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd"
                });

            SignSuccessDocsCommand command = new(offerid);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.ZapsignNoToken), result.FirstError.Description);
        }

        [Fact]
        public async Task HandleSignSuccessDocs_Sucess_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();
            Guid payerid = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            CatalogItemInfo EmailPort = new CatalogItemInfo() { Name = "EMAIL Port", Descripton = "587" };
            CatalogItemInfo EMAILServer = new CatalogItemInfo() { Name = "EMAIL Server", Descripton = "smtp.gmail.com" };
            CatalogItemInfo EMAILUser = new CatalogItemInfo() { Name = "EMAIL User", Descripton = "yourInvoicenotification@gmail.com" };
            CatalogItemInfo EMAILPasProjrd = new CatalogItemInfo() { Name = "EMAIL PasProjrd", Descripton = "ghkohgjmxfepfnbb" };
            CatalogItemInfo EMAILFrom = new CatalogItemInfo() { Name = "EMAIL From", Descripton = "yourInvoicenotification@gmail.com" };
            CatalogItemInfo EMAILSender = new CatalogItemInfo() { Name = "EMAIL Sender", Descripton = "yourInvoicenotification@gmail.com" };

            yourInvoice.Offer.Domain.Offer offer = new(offerid, payerid, userId, DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress);
            Document MoneyTransferInstruction = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.MoneyTransferInstruction, false, "url");
            Document CommercialOffer = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.CommercialOffer, false, "url", "1 mb", "token");
            Document Endorsement = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.Endorsement, false, "url");
            Document EndorsementNotification = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.EndorsementNotification, false, "url");
            Document MoneyTransferInstructionExcel = new(Guid.NewGuid(), offerid, null, "nombre", CatalogCode_DocumentType.MoneyTransferInstructionExcel, false, "url");

            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(new yourInvoice.Offer.Domain.Users.User(userId, 2, "890104521", "test name", Guid.NewGuid(), "72335847",
                "Cali", "Gerente", "Calle de prueba 123", "3015433443", "test@test.com", "Cali", Guid.NewGuid(), Guid.NewGuid(), "Licores del valle",
                "97654234", "5", "8725426273", "Cali", "Cali", true, DateTime.UtcNow, Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()));
            _mockRepository.Setup(x => x.GetByIdAsync(offerid)).ReturnsAsync(offer);
            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync("ruta");
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(offerid)).ReturnsAsync(new List<Document>() { MoneyTransferInstruction, CommercialOffer, Endorsement, EndorsementNotification, MoneyTransferInstructionExcel });
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "100" });
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(CatalogCode_DatayourInvoice.EmailAdmin)).ReturnsAsync(new CatalogItemInfo() { Descripton = "test@test.com" });
            _mockUserRepository.Setup(x => x.GetEmailRoleAsync(CatalogCode_UserRole.Administrator)).ReturnsAsync("test@test.com");

            _mockCatalogBusiness.Setup(x => x.ListByCatalogAsync("EMAILyourInvoice")).ReturnsAsync(new List<CatalogItemInfo>() { EmailPort, EMAILFrom, EMAILPasProjrd, EMAILSender, EMAILServer, EMAILUser });

            _mockIMediator.Setup(m => m.Send(It.IsAny<AddHistoricalCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Notification()).Verifiable("Notificacion enviada");
            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync(new MemoryStream());
            _invoiceRepository.Setup(c => c.GetInvoiceSumPayerSellerAsync(It.IsAny<Guid>())).ReturnsAsync(new InvoiceSumPayerSellerResponse { InvoiceSum = 1000, PayerName = "Payer", SellerName = "Seller" });
            _invoiceRepository.Setup(x => x.ListToGenerateExcel(offerid)).Returns(new List<yourInvoice.Offer.Domain.Invoices.Queries.InvoiceListGenerateExcelResponse>()
            { new yourInvoice.Offer.Domain.Invoices.Queries.InvoiceListGenerateExcelResponse()
            {  } });

            _mockZapsign.Setup(x => x.GetDetailAsync(It.IsAny<string>())).ReturnsAsync(
                new ZapsignFileDetailResponse()
                {
                    extra_docs = new List<ZapsignFileDetailExtraDocResponse>() {
                            new ZapsignFileDetailExtraDocResponse() { name = "Instruccion de Giro.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" },
                    new ZapsignFileDetailExtraDocResponse() { name = "Documento de Endoso.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" },
                    new ZapsignFileDetailExtraDocResponse() { name = "Notificacion de Endoso.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" }},
                    signed_file = "http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd"
                });

            SignSuccessDocsCommand command = new(offerid);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            Assert.True(result.Value);
        }

        [Fact]
        public async Task HandleSignSuccessDocs_NoOffer_UnSuccess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();

            SignSuccessDocsCommand command = new(offerid);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.OfferNotExist), result.FirstError.Description);
        }
    }
}