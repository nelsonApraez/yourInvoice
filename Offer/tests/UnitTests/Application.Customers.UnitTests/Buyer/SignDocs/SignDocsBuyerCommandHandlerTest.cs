///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Application.Customer.UnitTest.User;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Common.Integration.ZapSign;
using yourInvoice.Offer.Application.Buyer.SignDocs;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Buyer.SignDocs
{
    public class SignDocsBuyerCommandHandlerTest
    {
        private readonly Mock<IOfferRepository> _mockRepository;
        private readonly Mock<IZapsign> _mockZapsign;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IDocumentRepository> _mockDocumentRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IStorage> _mockStorage;
        private readonly Mock<ICatalogBusiness> _mockCatalogBusiness;
        private readonly Mock<IInvoiceDispersionRepository> _invoiceDispersionRepository;
        private readonly Mock<ISystem> _mockISystem;
        private SignDocsBuyerCommandHandler _handler;

        public SignDocsBuyerCommandHandlerTest()
        {
            _mockCatalogBusiness = new Mock<ICatalogBusiness>();
            _mockRepository = new Mock<IOfferRepository>();
            _mockZapsign = new Mock<IZapsign>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockDocumentRepository = new Mock<IDocumentRepository>();
            _mockStorage = new Mock<IStorage>();
            _invoiceDispersionRepository = new Mock<IInvoiceDispersionRepository>();
            _mockISystem = new Mock<ISystem>();
        }

        [Fact]
        public async Task HandleSignDocsBuyer_Signed_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();
            Guid payerid = UserData.GetUser.Id;
            Guid userId = UserData.GetUser.Id;
            int offerNumber = 0;
            Guid buyerId = UserData.GetUser.Id;

            _invoiceDispersionRepository.Setup(x => x.GetByOfferNumberAndBuyerIdAsync(It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(new InvoiceDispersion(
                    Guid.NewGuid(), offerNumber, buyerId, It.IsAny<Guid>(), payerid, new DateTime(), new DateTime(), 1, "1", "1", 1, 1, 10, 10, new char(), true, Guid.NewGuid(), new DateTime(), 0, new DateTime(), true
                    , new DateTime(), new DateTime(), 0, new DateTime(), Guid.NewGuid()));
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
                "Cali", "Gerente", "Calle de prueba 123", "3015433443", "test@test.com", "Cali", Guid.NewGuid(), Guid.NewGuid(), "Licores del valle", "97654234", "5", "8725426273", "Cali", "Cali", true, DateTime.UtcNow, Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()));
            _mockRepository.Setup(x => x.GetByConsecutiveAsync(offerNumber)).ReturnsAsync(offer);
            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync("ruta");
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(offerid)).ReturnsAsync(new List<Document>() { MoneyTransferInstruction,
                CommercialOffer, Endorsement, EndorsementNotification, CommercialOfferBuyer, MoneyTransferInstructionBuyer, TransferSupportBuyer, PurchaseCertificate });
            _mockZapsign.Setup(x => x.CreateDocAsync(It.IsAny<ZapsignFileRequest>())).ReturnsAsync(
                new ZapsignFileResponse() { token = "token", status = "pending", signers = new List<ZapsignSignerResponse>() { new ZapsignSignerResponse() { token = "token", sign_url = "url", status = "pending" } } });
            _mockZapsign.Setup(x => x.AddAttachmentAsync("token", It.IsAny<ZapsignFileAttachmentRequest>()));
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "info" });
            _mockStorage.Setup(x => x.DownloadAsync(It.IsAny<string>())).ReturnsAsync(new MemoryStream());
            SignDocsBuyerCommand command = new(offerNumber);

            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new SignDocsBuyerCommandHandler(_mockUnitOfWork.Object, _mockStorage.Object, _mockRepository.Object,
                _mockZapsign.Object, _mockUserRepository.Object, _mockDocumentRepository.Object, _mockCatalogBusiness.Object, _invoiceDispersionRepository.Object, _mockISystem.Object);
            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.Equal("token", result.Value.Token);
        }

        [Fact]
        public async Task HandleSignDocsBuyer_NoResponseZapsign_Error()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();
            Guid payerid = UserData.GetUser.Id;
            Guid userId = UserData.GetUser.Id;
            int offerNumber = 0;
            Guid buyerId = UserData.GetUser.Id;

            _invoiceDispersionRepository.Setup(x => x.GetByOfferNumberAndBuyerIdAsync(offerNumber, buyerId)).ReturnsAsync(new InvoiceDispersion(
                    Guid.NewGuid(), offerNumber, buyerId, It.IsAny<Guid>(), payerid, new DateTime(), new DateTime(), 1, "1", "1", 1, 1, 10, 10, new char(), true, Guid.NewGuid(), new DateTime(), 0, new DateTime(), true
                    , new DateTime(), new DateTime(), 0, new DateTime(), Guid.NewGuid()));
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
                "Cali", "Gerente", "Calle de prueba 123", "3015433443", "test@test.com", "Cali", Guid.NewGuid(), Guid.NewGuid(), "Licores del valle", "97654234", "5", "8725426273", "Cali", "Cali", true, DateTime.UtcNow, Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()));
            _mockRepository.Setup(x => x.GetByConsecutiveAsync(offerNumber)).ReturnsAsync(offer);
            _mockStorage.Setup(x => x.UploadAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync("ruta");
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(offerid)).ReturnsAsync(new List<Document>() {  MoneyTransferInstruction,
                CommercialOffer, Endorsement, EndorsementNotification, CommercialOfferBuyer, MoneyTransferInstructionBuyer, TransferSupportBuyer, PurchaseCertificate });
            _mockZapsign.Setup(x => x.CreateDocAsync(It.IsAny<ZapsignFileRequest>())).ReturnsAsync(
                new ZapsignFileResponse() { signers = new List<ZapsignSignerResponse>() { new ZapsignSignerResponse() { token = "token", sign_url = "url" } } });
            _mockZapsign.Setup(x => x.AddAttachmentAsync("token", It.IsAny<ZapsignFileAttachmentRequest>()));
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "info" });
            _mockStorage.Setup(x => x.DownloadAsync(It.IsAny<string>())).ReturnsAsync(new MemoryStream());
            SignDocsBuyerCommand command = new(offerNumber);

            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new SignDocsBuyerCommandHandler(_mockUnitOfWork.Object, _mockStorage.Object, _mockRepository.Object,
                _mockZapsign.Object, _mockUserRepository.Object, _mockDocumentRepository.Object, _mockCatalogBusiness.Object, _invoiceDispersionRepository.Object, _mockISystem.Object);

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
        public async Task HandleSignDocsBuyer_NoOffer_UnSuccess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            int offerNumber = 0;
            Guid buyerId = Guid.NewGuid();
            SignDocsBuyerCommand command = new(offerNumber);

            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new SignDocsBuyerCommandHandler(_mockUnitOfWork.Object, _mockStorage.Object, _mockRepository.Object,
                _mockZapsign.Object, _mockUserRepository.Object, _mockDocumentRepository.Object, _mockCatalogBusiness.Object, _invoiceDispersionRepository.Object, _mockISystem.Object);

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