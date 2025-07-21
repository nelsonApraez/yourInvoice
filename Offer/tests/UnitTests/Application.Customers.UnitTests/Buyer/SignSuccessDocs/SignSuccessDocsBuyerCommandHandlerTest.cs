///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Application.Customer.UnitTest.User;
using MediatR;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Common.Integration.ZapSign;
using yourInvoice.Offer.Application.Buyer.SignSuccessDocs;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Buyer.SignSuccessDocs
{
    public class SignSuccessDocsBuyerCommandHandlerTest
    {
        private readonly Mock<IOfferRepository> _mockRepository;
        private readonly Mock<IZapsign> _mockZapsign;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IDocumentRepository> _mockDocumentRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IStorage> _mockStorage;
        private readonly Mock<ICatalogBusiness> _mockCatalogBusiness;
        private readonly Mock<IInvoiceDispersionRepository> _invoiceDispersionRepository;
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ISystem> _mockISystem;
        private SignSuccessDocsBuyerCommandHandler _handler;

        public SignSuccessDocsBuyerCommandHandlerTest()
        {
            _mockCatalogBusiness = new Mock<ICatalogBusiness>();
            _mockRepository = new Mock<IOfferRepository>();
            _mockZapsign = new Mock<IZapsign>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockDocumentRepository = new Mock<IDocumentRepository>();
            _mockStorage = new Mock<IStorage>();
            _invoiceDispersionRepository = new Mock<IInvoiceDispersionRepository>();
            _mediator = new Mock<IMediator>();
            _mockISystem = new Mock<ISystem>();
        }

        [Fact]
        public async Task HandleSignSuccessDocs_NoToken_Error()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();
            Guid payerid = UserData.GetUser.Id;
            Guid userId = UserData.GetUser.Id;

            int offerNumber = 0;
            Guid buyerId = UserData.GetUser.Id;

            CatalogItemInfo EmailPort = new CatalogItemInfo() { Name = "EMAIL Port", Descripton = "587" };
            CatalogItemInfo EMAILServer = new CatalogItemInfo() { Name = "EMAIL Server", Descripton = "smtp.gmail.com" };
            CatalogItemInfo EMAILUser = new CatalogItemInfo() { Name = "EMAIL User", Descripton = "yourInvoicenotification@gmail.com" };
            CatalogItemInfo EMAILPasProjrd = new CatalogItemInfo() { Name = "EMAIL PasProjrd", Descripton = "ghkohgjmxfepfnbb" };
            CatalogItemInfo EMAILFrom = new CatalogItemInfo() { Name = "EMAIL From", Descripton = "yourInvoicenotification@gmail.com" };
            CatalogItemInfo EMAILSender = new CatalogItemInfo() { Name = "EMAIL Sender", Descripton = "yourInvoicenotification@gmail.com" };

            _invoiceDispersionRepository.Setup(x => x.GetByOfferNumberAndBuyerIdAsync(offerNumber, buyerId)).ReturnsAsync(new InvoiceDispersion(
        Guid.NewGuid(), offerNumber, buyerId, It.IsAny<Guid>(), payerid, new DateTime(), new DateTime(), 1, "1", "1", 1, 1, 10, 10, new char(), true, Guid.NewGuid(), new DateTime(), 0, new DateTime(), true
        , new DateTime(), new DateTime(), 0, new DateTime(), Guid.NewGuid()));
            _invoiceDispersionRepository.Setup(x => x.FindByOfferNumberAndBuyerIdAsync(offerNumber, buyerId)).ReturnsAsync(new List<InvoiceDispersion>());
            _invoiceDispersionRepository.Setup(x => x.GetPurchasePercentageAsync(offerNumber, default)).ReturnsAsync(100);
            yourInvoice.Offer.Domain.Offer offer = new(offerid, payerid, userId, DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress);

            Document MoneyTransferInstruction = new(Guid.NewGuid(), offerid, buyerId, "nombre", CatalogCode_DocumentType.MoneyTransferInstruction, false, "url");
            Document CommercialOffer = new(Guid.NewGuid(), offerid, buyerId, "nombre", CatalogCode_DocumentType.CommercialOffer, false, "url");
            Document Endorsement = new(Guid.NewGuid(), offerid, buyerId, "nombre", CatalogCode_DocumentType.Endorsement, false, "url");
            Document EndorsementNotification = new(Guid.NewGuid(), offerid, buyerId, "nombre", CatalogCode_DocumentType.EndorsementNotification, false, "url");

            Document CommercialOfferBuyer = new(Guid.NewGuid(), offerid, buyerId, "nombre", CatalogCode_DocumentType.CommercialOfferBuyer, false, "url");
            Document MoneyTransferInstructionBuyer = new(Guid.NewGuid(), offerid, buyerId, "nombre", CatalogCode_DocumentType.MoneyTransferInstructionBuyer, false, "url");
            Document TransferSupportBuyer = new(Guid.NewGuid(), offerid, buyerId, "nombre", CatalogCode_DocumentType.TransferSupportBuyer, false, "url");
            Document PurchaseCertificate = new(Guid.NewGuid(), offerid, buyerId, "nombre", CatalogCode_DocumentType.PurchaseCertificate, false, "url");

            _mockUserRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new yourInvoice.Offer.Domain.Users.User(userId, 2, "890104521", "test name", Guid.NewGuid(), "72335847",
                "Cali", "Gerente", "Calle de prueba 123", "3015433443", "test@test.com", "Cali", Guid.NewGuid(), Guid.NewGuid(), "Licores del valle",
                "97654234", "5", "8725426273", "Cali", "Cali", true, DateTime.UtcNow, Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()));
            _mockRepository.Setup(x => x.GetByConsecutiveAsync(offerNumber)).ReturnsAsync(offer);
            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync("ruta");
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(offerid)).ReturnsAsync(new List<Document>() { MoneyTransferInstruction,
                CommercialOffer, Endorsement, EndorsementNotification, CommercialOfferBuyer, MoneyTransferInstructionBuyer, TransferSupportBuyer, PurchaseCertificate });
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "100" });
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(CatalogCode_DatayourInvoice.EmailAdmin)).ReturnsAsync(new CatalogItemInfo() { Descripton = "test@test.com" });
            _mockUserRepository.Setup(x => x.GetEmailRoleAsync(CatalogCode_UserRole.Administrator)).ReturnsAsync("test@test.com");

            _mockCatalogBusiness.Setup(x => x.ListByCatalogAsync("EMAILyourInvoice")).ReturnsAsync(new List<CatalogItemInfo>() { EmailPort, EMAILFrom, EMAILPasProjrd, EMAILSender, EMAILServer, EMAILUser });

            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync(new MemoryStream());

            _mockZapsign.Setup(x => x.GetDetailAsync(It.IsAny<string>())).ReturnsAsync(
                new ZapsignFileDetailResponse()
                {
                    extra_docs = new List<ZapsignFileDetailExtraDocResponse>() { new ZapsignFileDetailExtraDocResponse() { name = "InstruccionGiroInversionista.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" },
                            new ZapsignFileDetailExtraDocResponse() { name = "CertificadoCompra.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" },
                    new ZapsignFileDetailExtraDocResponse() { name = "OfertaMercantilAceptada.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" }},
                    signed_file = "http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd"
                });

            SignSuccessDocsBuyerCommand command = new(offerNumber);

            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);

            _handler = new SignSuccessDocsBuyerCommandHandler(_mockUnitOfWork.Object, _mockStorage.Object, _mockRepository.Object,
                _mockZapsign.Object, _mockUserRepository.Object, _mockDocumentRepository.Object, _mockCatalogBusiness.Object, _mediator.Object, _invoiceDispersionRepository.Object, _mockISystem.Object);

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
            Guid payerid = UserData.GetUser.Id;
            Guid userId = UserData.GetUser.Id;

            int offerNumber = 0;
            Guid buyerId = UserData.GetUser.Id;

            CatalogItemInfo EmailPort = new CatalogItemInfo() { Name = "EMAIL Port", Descripton = "587" };
            CatalogItemInfo EMAILServer = new CatalogItemInfo() { Name = "EMAIL Server", Descripton = "smtp.gmail.com" };
            CatalogItemInfo EMAILUser = new CatalogItemInfo() { Name = "EMAIL User", Descripton = "yourInvoicenotification@gmail.com" };
            CatalogItemInfo EMAILPasProjrd = new CatalogItemInfo() { Name = "EMAIL PasProjrd", Descripton = "ghkohgjmxfepfnbb" };
            CatalogItemInfo EMAILFrom = new CatalogItemInfo() { Name = "EMAIL From", Descripton = "yourInvoicenotification@gmail.com" };
            CatalogItemInfo EMAILSender = new CatalogItemInfo() { Name = "EMAIL Sender", Descripton = "yourInvoicenotification@gmail.com" };

            _invoiceDispersionRepository.Setup(x => x.GetByOfferNumberAndBuyerIdAsync(offerNumber, buyerId)).ReturnsAsync(new InvoiceDispersion(
        Guid.NewGuid(), offerNumber, buyerId, It.IsAny<Guid>(), payerid, new DateTime(), new DateTime(), 1, "1", "1", 1, 1, 10, 10, new char(), true, Guid.NewGuid(), new DateTime(), 0, new DateTime(), true
        , new DateTime(), new DateTime(), 0, new DateTime(), Guid.NewGuid()));
            _invoiceDispersionRepository.Setup(x => x.FindByOfferNumberAndBuyerIdAsync(offerNumber, buyerId)).ReturnsAsync(new List<InvoiceDispersion>());
            _invoiceDispersionRepository.Setup(x => x.GetPurchasePercentageAsync(offerNumber, default)).ReturnsAsync(100);
            yourInvoice.Offer.Domain.Offer offer = new(offerid, payerid, userId, DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress);
            Document MoneyTransferInstruction = new(Guid.NewGuid(), offerid, buyerId, "nombre", CatalogCode_DocumentType.MoneyTransferInstruction, false, "url");
            Document CommercialOffer = new(Guid.NewGuid(), offerid, buyerId, "nombre", CatalogCode_DocumentType.CommercialOffer, false, "url", "1 mb", "token");
            Document Endorsement = new(Guid.NewGuid(), offerid, buyerId, "nombre", CatalogCode_DocumentType.Endorsement, false, "url");
            Document EndorsementNotification = new(Guid.NewGuid(), offerid, buyerId, "nombre", CatalogCode_DocumentType.EndorsementNotification, false, "url");

            Document CommercialOfferBuyer = new(Guid.NewGuid(), offerid, buyerId, "nombre", CatalogCode_DocumentType.CommercialOfferBuyer, false, "url", "1 mb", "token");
            Document MoneyTransferInstructionBuyer = new(Guid.NewGuid(), offerid, buyerId, "nombre", CatalogCode_DocumentType.MoneyTransferInstructionBuyer, false, "url");
            Document TransferSupportBuyer = new(Guid.NewGuid(), offerid, buyerId, "nombre", CatalogCode_DocumentType.TransferSupportBuyer, false, "url");
            Document PurchaseCertificate = new(Guid.NewGuid(), offerid, buyerId, "nombre", CatalogCode_DocumentType.PurchaseCertificate, false, "url");

            _mockUserRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new yourInvoice.Offer.Domain.Users.User(userId, 2, "890104521", "test name", Guid.NewGuid(), "72335847",
                "Cali", "Gerente", "Calle de prueba 123", "3015433443", "test@test.com", "Cali", Guid.NewGuid(), Guid.NewGuid(), "Licores del valle",
                "97654234", "5", "8725426273", "Cali", "Cali", true, DateTime.UtcNow, Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()));
            _mockRepository.Setup(x => x.GetByConsecutiveAsync(offerNumber)).ReturnsAsync(offer);
            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync("ruta");
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(offerid)).ReturnsAsync(new List<Document>() {  MoneyTransferInstruction,
                CommercialOffer, Endorsement, EndorsementNotification, CommercialOfferBuyer, MoneyTransferInstructionBuyer, TransferSupportBuyer, PurchaseCertificate });
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "100" });
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(CatalogCode_DatayourInvoice.EmailAdmin)).ReturnsAsync(new CatalogItemInfo() { Descripton = "test@test.com" });
            _mockUserRepository.Setup(x => x.GetEmailRoleAsync(CatalogCode_UserRole.Administrator)).ReturnsAsync("test@test.com");

            _mockCatalogBusiness.Setup(x => x.ListByCatalogAsync("EMAILyourInvoice")).ReturnsAsync(new List<CatalogItemInfo>() { EmailPort, EMAILFrom, EMAILPasProjrd, EMAILSender, EMAILServer, EMAILUser });

            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync(new MemoryStream());

            _mockZapsign.Setup(x => x.GetDetailAsync(It.IsAny<string>())).ReturnsAsync(
                new ZapsignFileDetailResponse()
                {
                    extra_docs = new List<ZapsignFileDetailExtraDocResponse>() { new ZapsignFileDetailExtraDocResponse() { name = "Instruccion de Giro Inversionista.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" },
                            new ZapsignFileDetailExtraDocResponse() { name = "Certificacion de Compra.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" },
                    new ZapsignFileDetailExtraDocResponse() { name = "Aceptacion Oferta Mercantil e Instruccion de giro.pdf", signed_file ="http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd" }},
                    signed_file = "http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd"
                });

            SignSuccessDocsBuyerCommand command = new(offerNumber);

            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);

            _handler = new SignSuccessDocsBuyerCommandHandler(_mockUnitOfWork.Object, _mockStorage.Object, _mockRepository.Object,
                _mockZapsign.Object, _mockUserRepository.Object, _mockDocumentRepository.Object, _mockCatalogBusiness.Object, _mediator.Object, _invoiceDispersionRepository.Object, _mockISystem.Object);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.True(result.Value);
        }

        [Fact]
        public async Task HandleSignSuccessDocs_NoOffer_UnSuccess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            int offerNumber = 0;
            Guid buyerId = Guid.NewGuid();
            SignSuccessDocsBuyerCommand command = new(offerNumber);

            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);

            _handler = new SignSuccessDocsBuyerCommandHandler(_mockUnitOfWork.Object, _mockStorage.Object, _mockRepository.Object,
                _mockZapsign.Object, _mockUserRepository.Object, _mockDocumentRepository.Object, _mockCatalogBusiness.Object, _mediator.Object, _invoiceDispersionRepository.Object, _mockISystem.Object);

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